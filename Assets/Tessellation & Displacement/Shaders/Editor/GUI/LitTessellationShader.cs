using System;
using UnityEngine;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    internal class LitTessellationShader : BaseShaderGUI
    {
        private const string k_KeyPrefix = "LightweightRP:Material:UI_State:";

        // Header foldout states
        private SavedBool m_GeometryFoldout;

        // Properties
        private LitTessellationGUI.LitTessellationProperties litProperties;

        // on open event
        public override void OnOpenGUI(Material material, MaterialEditor materialEditor)
        {
            var m_HeaderStateKey = k_KeyPrefix + material.shader.name; // Create key string for editor prefs
            m_GeometryFoldout = new SavedBool($"{m_HeaderStateKey}.GeometryOptionsFoldout", true);

            base.OnOpenGUI(material, materialEditor);
        }

        // collect properties from the material properties
        public override void FindProperties(MaterialProperty[] properties)
        {
            base.FindProperties(properties);
            litProperties = new LitTessellationGUI.LitTessellationProperties(properties);
        }

        // material changed check
        public override void MaterialChanged(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            SetMaterialKeywords(material, LitTessellationGUI.SetMaterialKeywords);
        }

        // material main surface options
        public override void DrawSurfaceOptions(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            // Use default labelWidth
            EditorGUIUtility.labelWidth = 0f;

            // Detect any changes to the material
            EditorGUI.BeginChangeCheck();
            if (litProperties.workflowMode != null)
            {
                DoPopup(UnityEditor.Rendering.Universal.ShaderGUI.LitGUI.Styles.workflowModeText, litProperties.workflowMode, Enum.GetNames(typeof(UnityEditor.Rendering.Universal.ShaderGUI.LitGUI.WorkflowMode)));
            }
            if (EditorGUI.EndChangeCheck())
            {
                foreach (var obj in blendModeProp.targets)
                    MaterialChanged((Material)obj);
            }
            base.DrawSurfaceOptions(material);
        }

        // material main surface inputs
        public override void DrawSurfaceInputs(Material material)
        {
            base.DrawSurfaceInputs(material);
            LitTessellationGUI.SurfaceInputs(litProperties, materialEditor, material);
            DrawEmissionProperties(material, true);
            DrawTileOffset(materialEditor, baseMapProp);
        }

        // material main advanced options
        public override void DrawAdvancedOptions(Material material)
        {
            if (litProperties.reflections != null && litProperties.highlights != null)
            {
                EditorGUI.BeginChangeCheck();
                {
                    materialEditor.ShaderProperty(litProperties.highlights, UnityEditor.Rendering.Universal.ShaderGUI.LitGUI.Styles.highlightsText);
                    materialEditor.ShaderProperty(litProperties.reflections, UnityEditor.Rendering.Universal.ShaderGUI.LitGUI.Styles.reflectionsText);
                    EditorGUI.BeginChangeCheck();
                }
            }

            base.DrawAdvancedOptions(material);
        }

        // material additional foldouts
        public override void DrawAdditionalFoldouts(Material material)
        {
            base.DrawAdditionalFoldouts(material);
            DrawGeometryFoldout(material);
        }
        
        // material geoemtry foldout
        public virtual void DrawGeometryFoldout(Material material)
        {
            m_GeometryFoldout.value = EditorGUILayout.BeginFoldoutHeaderGroup(m_GeometryFoldout.value, LitTessellationGUI.Styles.geometryLabel);
            if (m_GeometryFoldout.value)
            {
                LitTessellationGUI.GeometryInputs(litProperties, materialEditor, material);

                // geometry custom st
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = litProperties.geometryCustomST.hasMixedValue;
                var geometryCustomST = EditorGUILayout.Toggle(LitTessellationGUI.Styles.geometryCustomSTText, litProperties.geometryCustomST.floatValue == 1f) ? 1f : 0f;
                if (EditorGUI.EndChangeCheck())
                    litProperties.geometryCustomST.floatValue = geometryCustomST;
                EditorGUI.showMixedValue = false;

                // tile and offset
                if (geometryCustomST == 1f)
                    DrawTileOffset(materialEditor, litProperties.tessellationMap);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }


        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            // _Emission property is lost after assigning Standard shader to the material
            // thus transfer it before assigning the new shader
            if (material.HasProperty("_Emission"))
            {
                material.SetColor("_EmissionColor", material.GetColor("_Emission"));
            }

            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            if (oldShader == null || !oldShader.name.Contains("Legacy Shaders/"))
            {
                SetupMaterialBlendMode(material);
                return;
            }

            SurfaceType surfaceType = SurfaceType.Opaque;
            BlendMode blendMode = BlendMode.Alpha;
            if (oldShader.name.Contains("/Transparent/Cutout/"))
            {
                surfaceType = SurfaceType.Opaque;
                material.SetFloat("_AlphaClip", 1);
            }
            else if (oldShader.name.Contains("/Transparent/"))
            {
                // NOTE: legacy shaders did not provide physically based transparency
                // therefore Fade mode
                surfaceType = SurfaceType.Transparent;
                blendMode = BlendMode.Alpha;
            }
            material.SetFloat("_Surface", (float)surfaceType);
            material.SetFloat("_Blend", (float)blendMode);

            if (oldShader.name.Equals("Standard (Specular setup)"))
            {
                material.SetFloat("_WorkflowMode", (float)UnityEditor.Rendering.Universal.ShaderGUI.LitGUI.WorkflowMode.Specular);
                Texture texture = material.GetTexture("_SpecGlossMap");
                if (texture != null)
                    material.SetTexture("_MetallicSpecGlossMap", texture);
            }
            else
            {
                material.SetFloat("_WorkflowMode", (float)UnityEditor.Rendering.Universal.ShaderGUI.LitGUI.WorkflowMode.Metallic);
                Texture texture = material.GetTexture("_MetallicGlossMap");
                if (texture != null)
                    material.SetTexture("_MetallicSpecGlossMap", texture);
            }

            MaterialChanged(material);
        }
    }
}

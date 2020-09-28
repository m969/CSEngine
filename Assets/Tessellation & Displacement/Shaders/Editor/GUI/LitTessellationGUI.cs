using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    public static class LitTessellationGUI
    {
        public enum WorkflowMode
        {
            Specular = 0,
            Metallic
        }

        public enum SmoothnessMapChannel
        {
            SpecularMetallicAlpha,
            AlbedoAlpha,
        }

        public enum TessellationMode
        {
            Uniform = 0,
            EdgeLength = 1,
            Distance = 2
        }

        public enum TessellationPostPro
        {
            None,
            Phong
        }

        public enum HeightmapBlurMode
        {
            None = 0,
            Box = 1,
            Gaussian = 2
        }

        public enum VertexColorMode
        {
            Ignore = 0,
            Multiply = 1,
            //Additive = 2,
            //Replace = 3
        }

        public static class Styles
        {
            public static GUIContent workflowModeText = new GUIContent("Workflow Mode",
                "Select a workflow that fits your textures. Choose between Metallic or Specular.");

            public static GUIContent specularMapText =
                new GUIContent("Specular Map", "Sets and configures the map and color for the Specular workflow.");

            public static GUIContent metallicMapText =
                new GUIContent("Metallic Map", "Sets and configures the map for the Metallic workflow.");

            public static GUIContent smoothnessText = new GUIContent("Smoothness",
                "Controls the spread of highlights and reflections on the surface.");

            public static GUIContent smoothnessMapChannelText =
                new GUIContent("Source",
                    "Specifies where to sample a smoothness map from. By default, uses the alpha channel for your map.");

            public static GUIContent highlightsText = new GUIContent("Specular Highlights",
                "When enabled, the Material reflects the shine from direct lighting.");

            public static GUIContent reflectionsText =
                new GUIContent("Environment Reflections",
                    "When enabled, the Material samples reflections from the nearest Reflection Probes or Lighting Probe.");

            public static GUIContent occlusionText = new GUIContent("Occlusion Map",
                "Sets an occlusion map to simulate shadowing from ambient lighting.");


            public static GUIContent vertexColorModeText = new GUIContent("Vertex Color",
                "Sets the way vertex color is handle.");


            public static GUIContent geometryLabel = new GUIContent("Geometry Inputs",
                "These settings setups the geometry options of the shader.");

            public static GUIContent tessellationMapText = new GUIContent("Tessellation Map",
                "Tessellation map used to mask the tessellation itself. No map assigned will not mask the tessellation at all.");

            public static GUIContent tessellationModeText = new GUIContent("Mode",
                "Tessellation Mode.");

            public static GUIContent tessellationFactorText = new GUIContent("Factor",
                "Tessellation Factor.");

            public static GUIContent tessellationFactorMinText = new GUIContent("Factor Min Dist",
                "Tessellation Factor at Min Distance.");

            public static GUIContent tessellationFactorMaxText = new GUIContent("Factor Max Dist",
                "Tessellation Factor at Max Distance.");

            public static GUIContent tessellationDistanceMinText = new GUIContent("Distance Min",
                "Tessellation Distance Min.");

            public static GUIContent tessellationDistanceMaxText = new GUIContent("Distance Max",
                "Tessellation Distance Max.");

            public static GUIContent tessellationEdgeLengthText = new GUIContent("Edge Length",
                "Tessellation Edge Length.");

            public static GUIContent tessellationEdgeDistanceOffsetText = new GUIContent("Distance Offset",
                "Tessellation Edge Length Distance Offset.");

            public static GUIContent tessellationPostProText = new GUIContent("Post Processing",
                "Applies a mesh post tessellation processing technique");

            public static GUIContent tessellationPhongShapeText = new GUIContent("Shape",
                "Phong tessellation shape factor.");

            public static GUIContent tessellationTriangleClippingText = new GUIContent("Triangle Clipping",
                "Tessellation Triangle Clipping.");

            public static GUIContent tessellationTriangleClipBiasText = new GUIContent("Bias",
                "Tessellation Triangle Clip Bias.");


            public static GUIContent heightText = new GUIContent("Height Map",
                "Sets a height map to implement displacement.");

            public static GUIContent heightBaseText = new GUIContent("Base",
                "Sets a height map base (bias).");

            public static GUIContent heightBlurModeText = new GUIContent("Blur",
                "Sets a height map blur mode.");

            public static GUIContent heightBlurSizeText = new GUIContent("Size",
                "Sets a height map blur size.");

            public static GUIContent heightBlurSamplesText = new GUIContent("Samples",
                "Sets a height map blur samples.");

            public static GUIContent heightBlurGaussStandardDeviationText = new GUIContent("Standard Deviation",
                "Sets a height map gaussian blur standard deviaiton.");

            public static GUIContent geometryCustomSTText = new GUIContent("Use Custom Tiling and Offset",
                "Allows the geometry maps to use customs Tiling and Offsets.");


            public static readonly string[] metallicSmoothnessChannelNames = {"Metallic Alpha", "Albedo Alpha"};
            public static readonly string[] specularSmoothnessChannelNames = {"Specular Alpha", "Albedo Alpha"};
        }

        public struct LitTessellationProperties
        {
            // Surface Option Props
            public MaterialProperty workflowMode;

            // Surface Input Props
            public MaterialProperty metallic;
            public MaterialProperty specColor;
            public MaterialProperty metallicGlossMap;
            public MaterialProperty specGlossMap;
            public MaterialProperty smoothness;
            public MaterialProperty smoothnessMapChannel;
            public MaterialProperty bumpMapProp;
            public MaterialProperty bumpScaleProp;
            public MaterialProperty occlusionStrength;
            public MaterialProperty occlusionMap;

            // Geometry Inputs Props
            public MaterialProperty vertexColorMode;
            public MaterialProperty tessellationMap;
            public MaterialProperty tessellationScale;
            public MaterialProperty tessellationMode;
            public MaterialProperty tessellationFactor;
            public MaterialProperty tessellationFactorMin;
            public MaterialProperty tessellationFactorMax;
            public MaterialProperty tessellationDistanceMin;
            public MaterialProperty tessellationDistanceMax;
            public MaterialProperty tessellationEdgeLength;
            public MaterialProperty tessellationEdgeDistanceOffset;
            public MaterialProperty tessellationPostPro;
            public MaterialProperty tessellationPhongShape;
            public MaterialProperty tessellationTriangleClipping;
            public MaterialProperty tessellationTriangleClipBias;
            public MaterialProperty heightMap;
            public MaterialProperty heightStrength;
            public MaterialProperty heightBase;
            public MaterialProperty heightBlurMode;
            public MaterialProperty heightBlurSize;
            public MaterialProperty heightBlurSamples;
            public MaterialProperty heightBlurGaussStandardDeviation;
            public MaterialProperty geometryCustomST;

            // Advanced Props
            public MaterialProperty highlights;
            public MaterialProperty reflections;

            public LitTessellationProperties(MaterialProperty[] properties)
            {
                // Surface Option Props
                workflowMode = BaseShaderGUI.FindProperty("_WorkflowMode", properties, false);
                // Surface Input Props
                metallic = BaseShaderGUI.FindProperty("_Metallic", properties);
                specColor = BaseShaderGUI.FindProperty("_SpecColor", properties, false);
                metallicGlossMap = BaseShaderGUI.FindProperty("_MetallicGlossMap", properties);
                specGlossMap = BaseShaderGUI.FindProperty("_SpecGlossMap", properties, false);
                smoothness = BaseShaderGUI.FindProperty("_Smoothness", properties, false);
                smoothnessMapChannel = BaseShaderGUI.FindProperty("_SmoothnessTextureChannel", properties, false);
                bumpMapProp = BaseShaderGUI.FindProperty("_BumpMap", properties, false);
                bumpScaleProp = BaseShaderGUI.FindProperty("_BumpScale", properties, false);
                occlusionStrength = BaseShaderGUI.FindProperty("_OcclusionStrength", properties, false);
                occlusionMap = BaseShaderGUI.FindProperty("_OcclusionMap", properties, false);
                heightMap = BaseShaderGUI.FindProperty("_HeightMap", properties, false);
                // Advanced Props
                highlights = BaseShaderGUI.FindProperty("_SpecularHighlights", properties, false);
                reflections = BaseShaderGUI.FindProperty("_EnvironmentReflections", properties, false);
                // Geometry Input Props
                vertexColorMode = BaseShaderGUI.FindProperty("_VertexColorMode", properties);
                tessellationScale = BaseShaderGUI.FindProperty("_TessellationScale", properties);
                tessellationMap = BaseShaderGUI.FindProperty("_TessellationMap", properties);
                tessellationMode = BaseShaderGUI.FindProperty("_TessellationMode", properties);
                tessellationFactor = BaseShaderGUI.FindProperty("_TessellationFactor", properties);
                tessellationFactorMin = BaseShaderGUI.FindProperty("_TessellationFactorMin", properties);
                tessellationFactorMax = BaseShaderGUI.FindProperty("_TessellationFactorMax", properties);
                tessellationDistanceMin = BaseShaderGUI.FindProperty("_TessellationDistanceMin", properties);
                tessellationDistanceMax = BaseShaderGUI.FindProperty("_TessellationDistanceMax", properties);
                tessellationEdgeLength = BaseShaderGUI.FindProperty("_TessellationEdgeLength", properties);
                tessellationEdgeDistanceOffset = BaseShaderGUI.FindProperty("_TessellationEdgeDistanceOffset", properties);
                tessellationPostPro = BaseShaderGUI.FindProperty("_TessellationPostPro", properties);
                tessellationPhongShape = BaseShaderGUI.FindProperty("_TessellationPhongShape", properties);
                tessellationTriangleClipping = BaseShaderGUI.FindProperty("_TessellationTriangleClipping", properties);
                tessellationTriangleClipBias = BaseShaderGUI.FindProperty("_TessellationTriangleClipBias", properties);
                heightStrength = BaseShaderGUI.FindProperty("_HeightStrength", properties, false);
                heightBase = BaseShaderGUI.FindProperty("_HeightBase", properties, false);
                heightBlurMode = BaseShaderGUI.FindProperty("_HeightBlurMode", properties, false);
                heightBlurSize = BaseShaderGUI.FindProperty("_HeightBlurSize", properties, false);
                heightBlurSamples = BaseShaderGUI.FindProperty("_HeightBlurSamples", properties, false);
                heightBlurGaussStandardDeviation = BaseShaderGUI.FindProperty("_HeightBlurGaussStandardDeviation", properties, false);
                geometryCustomST = BaseShaderGUI.FindProperty("_GeometryCustomST", properties, false);
            }
        }

        public static void SurfaceInputs(LitTessellationProperties properties, MaterialEditor materialEditor, Material material)
        {
            DoMetallicSpecularArea(properties, materialEditor, material);

            BaseShaderGUI.DrawNormalArea(materialEditor, properties.bumpMapProp, properties.bumpScaleProp);

            if (properties.occlusionMap != null)
            {
                materialEditor.TexturePropertySingleLine(Styles.occlusionText, properties.occlusionMap,
                    properties.occlusionMap.textureValue != null ? properties.occlusionStrength : null);
            }
        }

        public static void GeometryInputs(LitTessellationProperties properties, MaterialEditor materialEditor, Material material)
        {
            // vertex color
            DoVertexColor(properties, materialEditor, material);

            // tesellation
            DoTessellation(properties, materialEditor, material);

            // height
            DoHeight(properties, materialEditor, material);
        }

        public static void DoVertexColor(LitTessellationProperties properties, MaterialEditor materialEditor, Material material)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = properties.vertexColorMode.hasMixedValue;
            var vertexColorMode = (LitTessellationGUI.VertexColorMode)properties.vertexColorMode.floatValue;
            vertexColorMode = (LitTessellationGUI.VertexColorMode)EditorGUILayout.EnumPopup(LitTessellationGUI.Styles.vertexColorModeText, vertexColorMode);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(LitTessellationGUI.Styles.vertexColorModeText.text);
                properties.vertexColorMode.floatValue = (float)vertexColorMode;
                SetMaterialKeywords(material);
            }
            EditorGUI.showMixedValue = false;
        }

        public static void DoTessellation(LitTessellationProperties properties, MaterialEditor materialEditor, Material material)
        {
            // mask
            materialEditor.TexturePropertySingleLine(LitTessellationGUI.Styles.tessellationMapText, properties.tessellationMap, properties.tessellationScale);

            EditorGUI.indentLevel++;
            EditorGUI.indentLevel++;

            // mode
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = properties.tessellationMode.hasMixedValue;
            var tessellationMode = (LitTessellationGUI.TessellationMode)properties.tessellationMode.floatValue;
            tessellationMode = (LitTessellationGUI.TessellationMode)EditorGUILayout.EnumPopup(LitTessellationGUI.Styles.tessellationModeText, tessellationMode);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(LitTessellationGUI.Styles.tessellationModeText.text);
                properties.tessellationMode.floatValue = (float)tessellationMode;
            }
            EditorGUI.showMixedValue = false;

            // modes
            if (tessellationMode == LitTessellationGUI.TessellationMode.Uniform)
            {
                // uniform
                EditorGUI.indentLevel++;

                // factor
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = properties.tessellationFactor.hasMixedValue;
                var tessellationFactor = EditorGUILayout.Slider(LitTessellationGUI.Styles.tessellationFactorText, properties.tessellationFactor.floatValue, 1f, 64f);
                if (EditorGUI.EndChangeCheck())
                    properties.tessellationFactor.floatValue = tessellationFactor;
                EditorGUI.showMixedValue = false;

                EditorGUI.indentLevel--;
            }
            else if (tessellationMode == LitTessellationGUI.TessellationMode.EdgeLength)
            {
                // edge length
                EditorGUI.indentLevel++;

                // edge length
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = properties.tessellationEdgeLength.hasMixedValue;
                var tessellationEdgeLength = EditorGUILayout.Slider(LitTessellationGUI.Styles.tessellationEdgeLengthText, properties.tessellationEdgeLength.floatValue, .05f, 32f);
                if (EditorGUI.EndChangeCheck())
                    properties.tessellationEdgeLength.floatValue = tessellationEdgeLength;
                EditorGUI.showMixedValue = false;

                // distance offset
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = properties.tessellationEdgeDistanceOffset.hasMixedValue;
                var tessellationDistanceOffset = EditorGUILayout.Slider(LitTessellationGUI.Styles.tessellationEdgeDistanceOffsetText, properties.tessellationEdgeDistanceOffset.floatValue, 0f, 10f);
                if (EditorGUI.EndChangeCheck())
                    properties.tessellationEdgeDistanceOffset.floatValue = tessellationDistanceOffset;
                EditorGUI.showMixedValue = false;

                EditorGUI.indentLevel--;
            }
            else if (tessellationMode == LitTessellationGUI.TessellationMode.Distance)
            {
                // distance
                EditorGUI.indentLevel++;

                // factor min
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = properties.tessellationFactorMin.hasMixedValue;
                var tessellationFactorMin = EditorGUILayout.Slider(LitTessellationGUI.Styles.tessellationFactorMinText, properties.tessellationFactorMin.floatValue, 1f, 64f);
                if (EditorGUI.EndChangeCheck())
                    properties.tessellationFactorMin.floatValue = tessellationFactorMin;
                EditorGUI.showMixedValue = false;

                // factor max
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = properties.tessellationFactorMax.hasMixedValue;
                var tessellationFactorMax = EditorGUILayout.Slider(LitTessellationGUI.Styles.tessellationFactorMaxText, properties.tessellationFactorMax.floatValue, 1f, 64f);
                if (EditorGUI.EndChangeCheck())
                    properties.tessellationFactorMax.floatValue = tessellationFactorMax;
                EditorGUI.showMixedValue = false;

                // distance min
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = properties.tessellationDistanceMin.hasMixedValue;
                var tessellationDistanceMin = EditorGUILayout.FloatField(LitTessellationGUI.Styles.tessellationDistanceMinText, properties.tessellationDistanceMin.floatValue);
                tessellationDistanceMin = Mathf.Max(0, tessellationDistanceMin);
                if (EditorGUI.EndChangeCheck())
                    properties.tessellationDistanceMin.floatValue = tessellationDistanceMin;
                EditorGUI.showMixedValue = false;

                // distance max
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = properties.tessellationDistanceMax.hasMixedValue;
                var tessellationDistanceMax = EditorGUILayout.FloatField(LitTessellationGUI.Styles.tessellationDistanceMaxText, properties.tessellationDistanceMax.floatValue);
                tessellationDistanceMax = Mathf.Max(tessellationDistanceMin, tessellationDistanceMax);
                if (EditorGUI.EndChangeCheck())
                    properties.tessellationDistanceMax.floatValue = tessellationDistanceMax;
                EditorGUI.showMixedValue = false;

                EditorGUI.indentLevel--;
            }

            // postpro
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = properties.tessellationPostPro.hasMixedValue;
            var tessellationPostPro = (LitTessellationGUI.TessellationPostPro)properties.tessellationPostPro.floatValue;
            tessellationPostPro = (LitTessellationGUI.TessellationPostPro)EditorGUILayout.EnumPopup(LitTessellationGUI.Styles.tessellationPostProText, tessellationPostPro);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(LitTessellationGUI.Styles.tessellationPostProText.text);
                properties.tessellationPostPro.floatValue = (float)tessellationPostPro;
            }
            EditorGUI.showMixedValue = false;

            // phong postpro
            if (tessellationPostPro == TessellationPostPro.Phong)
            {
                // phong tessellation
                EditorGUI.indentLevel++;

                // factor
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = properties.tessellationPhongShape.hasMixedValue;
                var tessellationPhongShape = EditorGUILayout.Slider(LitTessellationGUI.Styles.tessellationPhongShapeText, properties.tessellationPhongShape.floatValue, 0f, 1f);
                if (EditorGUI.EndChangeCheck())
                    properties.tessellationPhongShape.floatValue = tessellationPhongShape;
                EditorGUI.showMixedValue = false;

                EditorGUI.indentLevel--;
            }

            // triangle clipping
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = properties.tessellationTriangleClipping.hasMixedValue;
            var tessellationTriangleClipping = properties.tessellationTriangleClipping.floatValue == 1f;
            tessellationTriangleClipping = EditorGUILayout.Toggle(LitTessellationGUI.Styles.tessellationTriangleClippingText, tessellationTriangleClipping);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(LitTessellationGUI.Styles.tessellationTriangleClippingText.text);
                properties.tessellationTriangleClipping.floatValue = tessellationTriangleClipping ? 1f : 0f;
            }
            EditorGUI.showMixedValue = false;

            // triangle clip bias
            if (tessellationTriangleClipping)
            {
                EditorGUI.indentLevel++;

                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = properties.tessellationTriangleClipBias.hasMixedValue;
                var tessellationTriangleClipBias = EditorGUILayout.Slider(LitTessellationGUI.Styles.tessellationTriangleClipBiasText, properties.tessellationTriangleClipBias.floatValue, -1f, 1f);
                if (EditorGUI.EndChangeCheck())
                    properties.tessellationTriangleClipBias.floatValue = tessellationTriangleClipBias;
                EditorGUI.showMixedValue = false;

                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
        }

        public static void DoHeight(LitTessellationProperties properties, MaterialEditor materialEditor, Material material)
        {
            if (properties.heightMap != null)
            {
                materialEditor.TexturePropertySingleLine(Styles.heightText, properties.heightMap, properties.heightMap.textureValue != null ? properties.heightStrength : null);

                EditorGUI.indentLevel++;
                EditorGUI.indentLevel++;

                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = properties.heightBase.hasMixedValue;
                var heightBase = EditorGUILayout.Slider(Styles.heightBaseText, properties.heightBase.floatValue, -1f, 1f);
                if (EditorGUI.EndChangeCheck())
                    properties.heightBase.floatValue = heightBase;
                EditorGUI.showMixedValue = false;

                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = properties.heightBlurMode.hasMixedValue;
                var heightBlurMode = (LitTessellationGUI.HeightmapBlurMode)properties.heightBlurMode.floatValue;
                heightBlurMode = (LitTessellationGUI.HeightmapBlurMode)EditorGUILayout.EnumPopup(LitTessellationGUI.Styles.heightBlurModeText, heightBlurMode);
                if (EditorGUI.EndChangeCheck())
                {
                    materialEditor.RegisterPropertyChangeUndo(LitTessellationGUI.Styles.heightBlurModeText.text);
                    properties.heightBlurMode.floatValue = (float)heightBlurMode;
                    SetMaterialKeywords(material);
                }
                EditorGUI.showMixedValue = false;

                if (heightBlurMode != HeightmapBlurMode.None)
                {
                    EditorGUI.indentLevel++;

                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = properties.heightBlurSize.hasMixedValue;
                    var heightBlurSize = EditorGUILayout.Slider(Styles.heightBlurSizeText, properties.heightBlurSize.floatValue, 0f, 1.0f);
                    if (EditorGUI.EndChangeCheck())
                        properties.heightBlurSize.floatValue = heightBlurSize;
                    EditorGUI.showMixedValue = false;

                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = properties.heightBlurSamples.hasMixedValue;
                    var heightBlurSamples = EditorGUILayout.IntSlider(Styles.heightBlurSamplesText, (int)properties.heightBlurSamples.floatValue, 1, 60);
                    if (EditorGUI.EndChangeCheck())
                        properties.heightBlurSamples.floatValue = heightBlurSamples;
                    EditorGUI.showMixedValue = false;

                    if (heightBlurMode == HeightmapBlurMode.Gaussian)
                    {
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.showMixedValue = properties.heightBlurGaussStandardDeviation.hasMixedValue;
                        var heightBlurGaussStandardDeviation = EditorGUILayout.Slider(Styles.heightBlurGaussStandardDeviationText, properties.heightBlurGaussStandardDeviation.floatValue, 0.001f, 0.1f);
                        if (EditorGUI.EndChangeCheck())
                            properties.heightBlurGaussStandardDeviation.floatValue = heightBlurGaussStandardDeviation;
                        EditorGUI.showMixedValue = false;
                    }

                    EditorGUILayout.HelpBox("Heightmap blur may cause performance issues on heavy tessellated meshes. Avoid using it if possible.", MessageType.Warning);

                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }
        }

        public static void DoMetallicSpecularArea(LitTessellationProperties properties, MaterialEditor materialEditor, Material material)
        {
            string[] smoothnessChannelNames;
            bool hasGlossMap = false;
            if (properties.workflowMode == null ||
                (WorkflowMode) properties.workflowMode.floatValue == WorkflowMode.Metallic)
            {
                hasGlossMap = properties.metallicGlossMap.textureValue != null;
                smoothnessChannelNames = Styles.metallicSmoothnessChannelNames;
                materialEditor.TexturePropertySingleLine(Styles.metallicMapText, properties.metallicGlossMap,
                    hasGlossMap ? null : properties.metallic);
            }
            else
            {
                hasGlossMap = properties.specGlossMap.textureValue != null;
                smoothnessChannelNames = Styles.specularSmoothnessChannelNames;
                BaseShaderGUI.TextureColorProps(materialEditor, Styles.specularMapText, properties.specGlossMap,
                    hasGlossMap ? null : properties.specColor);
            }
            EditorGUI.indentLevel++;
            DoSmoothness(properties, material, smoothnessChannelNames);
            EditorGUI.indentLevel--;
        }

        public static void DoSmoothness(LitTessellationProperties properties, Material material, string[] smoothnessChannelNames)
        {
            var opaque = ((BaseShaderGUI.SurfaceType) material.GetFloat("_Surface") ==
                          BaseShaderGUI.SurfaceType.Opaque);
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = properties.smoothness.hasMixedValue;
            var smoothness = EditorGUILayout.Slider(Styles.smoothnessText, properties.smoothness.floatValue, 0f, 1f);
            if (EditorGUI.EndChangeCheck())
                properties.smoothness.floatValue = smoothness;
            EditorGUI.showMixedValue = false;

            if (properties.smoothnessMapChannel != null) // smoothness channel
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginDisabledGroup(!opaque);
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = properties.smoothnessMapChannel.hasMixedValue;
                var smoothnessSource = (int) properties.smoothnessMapChannel.floatValue;
                if (opaque)
                    smoothnessSource = EditorGUILayout.Popup(Styles.smoothnessMapChannelText, smoothnessSource,
                        smoothnessChannelNames);
                else
                    EditorGUILayout.Popup(Styles.smoothnessMapChannelText, 0, smoothnessChannelNames);
                if (EditorGUI.EndChangeCheck())
                    properties.smoothnessMapChannel.floatValue = smoothnessSource;
                EditorGUI.showMixedValue = false;
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

        public static SmoothnessMapChannel GetSmoothnessMapChannel(Material material)
        {
            int ch = (int) material.GetFloat("_SmoothnessTextureChannel");
            if (ch == (int) SmoothnessMapChannel.AlbedoAlpha)
                return SmoothnessMapChannel.AlbedoAlpha;

            return SmoothnessMapChannel.SpecularMetallicAlpha;
        }

        public static void SetMaterialKeywords(Material material)
        {
            // Note: keywords must be based on Material value not on MaterialProperty due to multi-edit & material animation
            // (MaterialProperty value might come from renderer material property block)
            var hasGlossMap = false;
            var isSpecularWorkFlow = false;
            var opaque = ((BaseShaderGUI.SurfaceType) material.GetFloat("_Surface") ==
                          BaseShaderGUI.SurfaceType.Opaque);
            if (material.HasProperty("_WorkflowMode"))
            {
                isSpecularWorkFlow = (WorkflowMode) material.GetFloat("_WorkflowMode") == WorkflowMode.Specular;
                if (isSpecularWorkFlow)
                    hasGlossMap = material.GetTexture("_SpecGlossMap") != null;
                else
                    hasGlossMap = material.GetTexture("_MetallicGlossMap") != null;
            }
            else
            {
                hasGlossMap = material.GetTexture("_MetallicGlossMap") != null;
            }

            CoreUtils.SetKeyword(material, "_SPECULAR_SETUP", isSpecularWorkFlow);

            CoreUtils.SetKeyword(material, "_METALLICSPECGLOSSMAP", hasGlossMap);

            if (material.HasProperty("_SpecularHighlights"))
                CoreUtils.SetKeyword(material, "_SPECULARHIGHLIGHTS_OFF",
                    material.GetFloat("_SpecularHighlights") == 0.0f);
            if (material.HasProperty("_EnvironmentReflections"))
                CoreUtils.SetKeyword(material, "_ENVIRONMENTREFLECTIONS_OFF",
                    material.GetFloat("_EnvironmentReflections") == 0.0f);
            if (material.HasProperty("_OcclusionMap"))
                CoreUtils.SetKeyword(material, "_OCCLUSIONMAP", material.GetTexture("_OcclusionMap"));
            if (material.HasProperty("_HeightMap"))
                CoreUtils.SetKeyword(material, "_HEIGHTMAP", material.GetTexture("_HeightMap"));

            if (material.HasProperty("_SmoothnessTextureChannel"))
            {
                CoreUtils.SetKeyword(material, "_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A",
                    GetSmoothnessMapChannel(material) == SmoothnessMapChannel.AlbedoAlpha && opaque);
            }

            var vertexColorMode = VertexColorMode.Ignore;
            if (material.HasProperty("_VertexColorMode"))
            {
                vertexColorMode = (VertexColorMode)material.GetFloat("_VertexColorMode");
            }
            CoreUtils.SetKeyword(material, "_VERTEX_COLOR_MULTIPLY", vertexColorMode == VertexColorMode.Multiply);
            //CoreUtils.SetKeyword(material, "_VERTEX_COLOR_ADDITIVE", vertexColorMode == VertexColorMode.Additive);
            //CoreUtils.SetKeyword(material, "_VERTEX_COLOR_REPLACE", vertexColorMode == VertexColorMode.Replace);

            if (material.HasProperty("_TessellationMap"))
            {
                CoreUtils.SetKeyword(material, "_TESSELLATIONMAP", material.GetTexture("_TessellationMap"));
            }

            var tessellationMode = TessellationMode.Uniform;
            if (material.HasProperty("_TessellationMode"))
            {
                tessellationMode = (TessellationMode)material.GetFloat("_TessellationMode");
            }
            CoreUtils.SetKeyword(material, "_TESSELLATION_EDGE", tessellationMode == TessellationMode.EdgeLength);
            CoreUtils.SetKeyword(material, "_TESSELLATION_DISTANCE", tessellationMode == TessellationMode.Distance);

            var tessellationPostPro = TessellationPostPro.None;
            if (material.HasProperty("_TessellationPostPro"))
            {
                tessellationPostPro = (TessellationPostPro)material.GetFloat("_TessellationPostPro");
            }
            CoreUtils.SetKeyword(material, "_TESSELLATION_PHONG", tessellationPostPro == TessellationPostPro.Phong);

            var tessellationTriangleClipping = false;
            if (material.HasProperty("_TessellationTriangleClipping"))
            {
                tessellationTriangleClipping = material.GetFloat("_TessellationTriangleClipping") == 1f;
            }
            CoreUtils.SetKeyword(material, "_TESSELLATION_CLIPPING", tessellationTriangleClipping);

            var heightBlurMode = HeightmapBlurMode.None;
            if (material.HasProperty("_HeightBlurMode"))
            {
                heightBlurMode = (HeightmapBlurMode)material.GetFloat("_HeightBlurMode");
            }
            CoreUtils.SetKeyword(material, "_HEIGHTMAP_BLUR_BOX", heightBlurMode == HeightmapBlurMode.Box);
            CoreUtils.SetKeyword(material, "_HEIGHTMAP_BLUR_GAUSS", heightBlurMode == HeightmapBlurMode.Gaussian);

            if (material.HasProperty("_GeometryCustomST"))
                CoreUtils.SetKeyword(material, "_GEOMETRY_CUSTOM_ST",
                    material.GetFloat("_GeometryCustomST") == 1.0f);
        }
    }
}

#ifndef UNIVERSAL_LIT_TESSELLATION_STRUCTS_INCLUDED
#define UNIVERSAL_LIT_TESSELLATION_STRUCTS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

#if VERTEX_POSITION_OS
#define HAS_UNITY_POSITION 1
#define UNITY_POSITION positionOS
#elif VERTEX_POSITION
#define HAS_UNITY_POSITION 1
#define UNITY_POSITION position
#endif

#if VERTEX_NORMAL_OS
#define HAS_UNITY_NORMAL 1
#define HAS_UNITY_OUTPUT_NORMAL 1
#define UNITY_NORMAL normalOS
#elif VERTEX_NORMAL
#define HAS_UNITY_NORMAL 1
#define HAS_UNITY_OUTPUT_NORMAL 1
#define UNITY_NORMAL normal
#else
#define HAS_UNITY_NORMAL 1
#define UNITY_NORMAL normalOS
#endif

#if VERTEX_TEXCOORD
#define HAS_UNITY_UV0 1
#define UNITY_UV0 texcoord
#elif VERTEX_UV0
#define HAS_UNITY_UV0 1
#define UNITY_UV0 uv0
#endif

#if VERTEX_LIGHTMAP_UV
#define HAS_UNITY_UV1 1
#define UNITY_UV1 lightmapUV
#elif VERTEX_UV1
#define HAS_UNITY_UV1 1
#define UNITY_UV1 uv1
#endif

#if VERTEX_UV2
#define HAS_UNITY_UV2 1
#define UNITY_UV2 uv2
#endif

#if VERTEX_TANGENT_OS
#define HAS_UNITY_TANGENT 1
#define UNITY_TANGENT tangentOS
#endif

#if _VERTEX_COLOR_MULTIPLY	// Pass material keyword
#if VERTEX_COLOR	// Pass struct property
#define HAS_UNITY_VERTEX_COLOR 1
#define UNITY_VERTEX_COLOR color
#endif
#endif

// The input structure of the tessellation
struct TessellationAttributes
{
#if HAS_UNITY_POSITION
	float4 UNITY_POSITION		: POSITION;
#endif
#if HAS_UNITY_NORMAL
	float3 UNITY_NORMAL			: NORMAL;
#endif
#if HAS_UNITY_TANGENT
	float4 UNITY_TANGENT		: TANGENT;
#endif
#if HAS_UNITY_UV0
	float2 UNITY_UV0			: TEXCOORD0;
#endif
#if HAS_UNITY_UV1
	float2 UNITY_UV1			: TEXCOORD1;
#endif
#if HAS_UNITY_UV2
	float2 UNITY_UV2			: TEXCOORD2;
#endif
#if HAS_UNITY_VERTEX_COLOR
	half4 UNITY_VERTEX_COLOR	: COLOR;
#endif

	UNITY_VERTEX_INPUT_INSTANCE_ID
};

// The output structure of the vertex shader
struct TessellationControlPoint
{
#if HAS_UNITY_POSITION
	float4 UNITY_POSITION		: INTERNALTESSPOS;
#endif
#if HAS_UNITY_NORMAL
	float3 UNITY_NORMAL			: NORMAL;
#endif
#if HAS_UNITY_TANGENT
	float4 UNITY_TANGENT		: TANGENT;
#endif
#if HAS_UNITY_UV0
	float2 UNITY_UV0			: TEXCOORD0;
#endif
#if HAS_UNITY_UV1
	float2 UNITY_UV1			: TEXCOORD1;
#endif
#if HAS_UNITY_UV2
	float2 UNITY_UV2			: TEXCOORD2;
#endif
#if HAS_UNITY_VERTEX_COLOR
	half4 UNITY_VERTEX_COLOR	: COLOR;
#endif

	UNITY_VERTEX_INPUT_INSTANCE_ID
};

// Additional fragment varyings
struct TessellatedVaryings
{
	Varyings nativeVaryings;

#if HAS_UNITY_VERTEX_COLOR
	half4 UNITY_VERTEX_COLOR	: COLOR;
#endif
};

#endif // UNIVERSAL_LIT_TESSELLATION_STRUCTS_INCLUDED

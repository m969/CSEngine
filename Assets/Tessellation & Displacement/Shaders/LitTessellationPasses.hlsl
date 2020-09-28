#ifndef UNIVERSAL_LIT_TESSELLATION_INCLUDED
#define UNIVERSAL_LIT_TESSELLATION_INCLUDED

#include "Library/Tessellation.hlsl"

// Vertex shader
TessellationControlPoint TessellatedVertex(TessellationAttributes input)
{
	TessellationControlPoint output;

	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_TRANSFER_INSTANCE_ID(input, output);

#if HAS_UNITY_POSITION
	output.UNITY_POSITION = input.UNITY_POSITION;
#endif
#if HAS_UNITY_NORMAL
	output.UNITY_NORMAL = input.UNITY_NORMAL;
#endif
#if HAS_UNITY_TANGENT
	output.UNITY_TANGENT = input.UNITY_TANGENT;
#endif
#if HAS_UNITY_UV0
	output.UNITY_UV0 = input.UNITY_UV0;
#endif
#if HAS_UNITY_UV1
	output.UNITY_UV1 = input.UNITY_UV1;
#endif
#if HAS_UNITY_UV2
	output.UNITY_UV2 = input.UNITY_UV2;
#endif
#if HAS_UNITY_VERTEX_COLOR
	output.UNITY_VERTEX_COLOR = input.UNITY_VERTEX_COLOR;
#endif

	return output;
}

// Hull shader
[maxtessfactor(MAX_TESSELLATION_FACTORS)]
[domain("tri")]						// Processing triangle face
[partitioning("fractional_odd")]	// The parameter type of the subdivided factor, can be "integer" which is used to represent the integer, or can be a floating point number "fractional_odd"
[outputtopology("triangle_cw")]		// Clockwise vertex arranged as the front of the triangle
[patchconstantfunc("HullConstant")] // The function that calculates the factor of the triangle facet is not a constant. Different triangle faces can have different values. A constant can be understood as a uniform value for the three vertices inside a triangle face.
[outputcontrolpoints(3)]			// Explicitly point out that each patch handles three vertex data
TessellationControlPoint TessellatedHull(
	InputPatch<TessellationControlPoint, 3> patch,
	uint id:SV_OutputControlPointID)
{
	return patch[id];
}

// Domain shader
[domain("tri")]	// Specified to handle the triangle face triangle
TessellatedVaryings TessellatedDomain(
	TessellationFactors tessFactors,
	const OutputPatch<TessellationControlPoint, 3> input,
	float3 barycentricCoordinates : SV_DomainLocation)
{
	// Program output
	TessellatedVaryings varyings = (TessellatedVaryings)0;

	// Native vertex program input
	Attributes data = (Attributes)0;

	UNITY_TRANSFER_INSTANCE_ID(input[0], data);

	// This uses a macro definition to reduce the writing of duplicate code
#define MY_DOMAIN_PROGRAM_INTERPOLATE(fieldName) input[0].fieldName * barycentricCoordinates.x + \
					input[1].fieldName * barycentricCoordinates.y + \
					input[2].fieldName * barycentricCoordinates.z;

#if HAS_UNITY_POSITION
	data.UNITY_POSITION = MY_DOMAIN_PROGRAM_INTERPOLATE(UNITY_POSITION)	// Interpolation calculates vertex coordinates
#endif
#if HAS_UNITY_NORMAL
	float3 outputNormal = MY_DOMAIN_PROGRAM_INTERPOLATE(UNITY_NORMAL)
#if HAS_UNITY_OUTPUT_NORMAL
	data.UNITY_NORMAL = outputNormal;									// Interpolation calculation normal
#endif
#endif
#if HAS_UNITY_TANGENT
	data.UNITY_TANGENT = MY_DOMAIN_PROGRAM_INTERPOLATE(UNITY_TANGENT)	// Interpolation calculation tangent
#endif
#if HAS_UNITY_UV0
	data.UNITY_UV0 = MY_DOMAIN_PROGRAM_INTERPOLATE(UNITY_UV0);			// Interpolation calculation UV0
#endif
#if HAS_UNITY_UV1
	data.UNITY_UV1 = MY_DOMAIN_PROGRAM_INTERPOLATE(UNITY_UV1)			// Interpolation calculation UV1
#endif
#if HAS_UNITY_UV2
	data.UNITY_UV2 = MY_DOMAIN_PROGRAM_INTERPOLATE(UNITY_UV2)			// Interpolation calculation UV2
#endif
#if HAS_UNITY_VERTEX_COLOR
	varyings.UNITY_VERTEX_COLOR = MY_DOMAIN_PROGRAM_INTERPOLATE(UNITY_VERTEX_COLOR)		// Interpolation calculation UV2
#endif

#ifdef _TESSELLATION_PHONG
	float3 p0 = mul(unity_ObjectToWorld, input[0].UNITY_POSITION).xyz;
	float3 p1 = mul(unity_ObjectToWorld, input[1].UNITY_POSITION).xyz;
	float3 p2 = mul(unity_ObjectToWorld, input[2].UNITY_POSITION).xyz;
	float3 n0 = TransformObjectToWorldNormal(input[0].UNITY_NORMAL);
	float3 n1 = TransformObjectToWorldNormal(input[1].UNITY_NORMAL);
	float3 n2 = TransformObjectToWorldNormal(input[2].UNITY_NORMAL);
	float3 positionWS = mul(unity_ObjectToWorld, data.UNITY_POSITION).xyz;
	positionWS = PhongTessellation(positionWS,
		p0, p1, p2, n0, n1, n2,
		barycentricCoordinates, 
		_TessellationPhongShape);
	data.UNITY_POSITION = mul(unity_WorldToObject, float4(positionWS, 1.0));
#endif

#if _HEIGHTMAP && HAS_UNITY_NORMAL && HAS_UNITY_UV0
#if WORLDSPACE_HEIGHTMAP_SAMPLING
#ifndef _TESSELLATION_PHONG
	float3 positionWS = mul(unity_ObjectToWorld, data.UNITY_POSITION).xyz;
#endif
	float3 normalWS = mul(unity_ObjectToWorld, outputNormal).xyz;
	data.UNITY_POSITION.xyz += normalize(outputNormal) * SampleHeightWorldCoordinates(positionWS, normalWS);
#else
	float2 samplingUV = TRANSFORM_TEX(data.UNITY_UV0, TESSELLATION_TRANSFORM_SOURCE);
	data.UNITY_POSITION.xyz += normalize(outputNormal) * SampleHeight(samplingUV);
#endif
#endif

	// Invoke native vertex program
	varyings.nativeVaryings = PassVertexProgram(data);	// Process the interpolation results, prepare the data needed in the rasterization phase

	return varyings;		
}

// Fragment shader
half4 TessellatedFragment(TessellatedVaryings input) : SV_Target
{
	half4 color = PassFragmentProgram(input.nativeVaryings);

#if HAS_UNITY_VERTEX_COLOR
	color *= input.UNITY_VERTEX_COLOR;
#endif

	return color;
}

#endif // UNIVERSAL_LIT_TESSELLATION_INCLUDED

#ifndef BLUR_INCLUDED
#define BLUR_INCLUDED

#define SAMPLE_TEXTURE2D_LOD_BLUR_BOX(textureName, samplerName, coord2, lod, size, samples) SampleLodBlurBox(textureName, samplerName, coord2, lod, size, samples)
#define SAMPLE_TEXTURE2D_LOD_BLUR_GAUSS(textureName, samplerName, coord2, lod, size, samples, deviation) SampleLodBlurGauss(textureName, samplerName, coord2, lod, size, samples, deviation)

#define E 2.71828182846

// Box Blur
half4 SampleLodBlurBox(TEXTURE2D(tex), SAMPLER(texSampler), float2 uv, int lod, float size, int samples)
{
	float4 color = 0;
	float sum = samples * 2;

	// Iterate over blur samples
	for (float index = 0; index < samples; index++)
	{
		// Get uv offset
		float offset = (index / (samples - 1) - 0.5) * size / 20.0;

		// Add color at position to color
		color += SAMPLE_TEXTURE2D_LOD(tex, texSampler, uv + float2(offset, 0), lod);
		color += SAMPLE_TEXTURE2D_LOD(tex, texSampler, uv + float2(0, offset), lod);
	}

	// Divide the sum of values by the amount of samples
	return color / sum;
}

// Gaussian Blur
half4 SampleLodBlurGauss(TEXTURE2D(tex), SAMPLER(texSampler), float2 uv, int lod, float size, int samples, float deviation)
{
	float4 color = 0;
	float sum = 0;

	for (float index = 0; index < samples; index++)
	{
		// Get uv offset
		float offset = (index / (samples - 1) - 0.5) * size / 20.0;

		// Calculate the result of the gaussian function
		float deviationSquared = deviation * deviation;
		float gauss = (1 / sqrt(2 * PI * deviationSquared)) * pow(E, -((offset * offset) / (2 * deviationSquared)));

		// Add result to sum
		sum += gauss * 2;

		// Multiply color with influence from gaussian function and add it to sum color
		color += SAMPLE_TEXTURE2D_LOD(tex, texSampler, uv + float2(offset, 0), lod) * gauss;
		color += SAMPLE_TEXTURE2D_LOD(tex, texSampler, uv + float2(0, offset), lod) * gauss;
	}

	// Divide the sum of values by the amount of samples
	return color / sum;
}

#endif //BLUR_INCLUDED

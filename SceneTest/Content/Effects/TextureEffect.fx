texture2D BasicTexture;
texture2D LightTexture;
sampler2D LightSampler = sampler_state {
	texture = <LightTexture>;
	minfilter = point;
	magfilter = point;
	mipfilter = point;
};
sampler BasicTextureSampler = sampler_state {
	texture = <BasicTexture>;
};


struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
};

float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color : COLOR0, float2 UV : TEXCOORD0) : COLOR0
{
	return saturate(/*tex2D(BasicTextureSampler, UV) * */tex2D(LightSampler, UV));
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_3 PixelShaderFunction();
	}
}

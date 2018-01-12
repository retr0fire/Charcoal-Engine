float4x4 World;
float4x4 View;
float4x4 Projection;

float3 DiffuseColor;

bool TextureEnabled;
texture BasicTexture;
sampler BasicTextureSampler = sampler_state {
texture = <BasicTexture>;
};



float FarPlane=400;
float NearPlane=0.1;






struct VertexShaderInput
{
	float4 Position : POSITION0;
};
struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 Depth : TEXCOORD0;
};
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	
	float4x4 viewProjection = mul(View, Projection);
	float4x4 worldViewProjection = mul(World, viewProjection);
	output.Position = mul(input.Position, worldViewProjection);

	output.Depth = output.Position.ww;
return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	return input.Depth.y/FarPlane;
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_4_0 VertexShaderFunction();
        PixelShader = compile ps_4_0 PixelShaderFunction();
    }
}
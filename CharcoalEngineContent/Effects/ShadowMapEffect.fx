float4x4 World;
float4x4 View;
float4x4 Projection;
float FarPlane;
float NearPlane;
float Alpha = 1;
bool TextureEnabled;
texture BasicTexture;
sampler BasicTextureSampler = sampler_state {
texture = <BasicTexture>;
};
struct VertexShaderInput
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float2 UV : TEXCOORD0;
};
struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float3 Depth : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float2 UV : TEXCOORD2;
};
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	float4x4 viewProjection = mul(View, Projection);
	float4x4 worldViewProjection = mul(World, viewProjection);
	output.Position = mul(input.Position, worldViewProjection);
	output.Normal = mul(input.Normal, World);
	output.Depth = output.Position;
	output.UV = input.UV;
return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float d = distance(input.Depth, float3(0, 0, 0));
	d -= NearPlane;
	d /= (FarPlane-NearPlane);
	float texturevalue = tex2D(BasicTextureSampler, input.UV).a;
	if (TextureEnabled == false)
		texturevalue = 1;

	if (texturevalue * Alpha < 1)
		return 0;
	return d;
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
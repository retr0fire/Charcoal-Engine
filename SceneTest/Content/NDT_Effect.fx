float4x4 World;
float4x4 View;
float4x4 Projection;

float3 DiffuseColor;
bool TextureEnabled;
texture BasicTexture;
//sampler BasicTextureSampler = sampler_state {
//	texture = <BasicTexture>;
//};
float FarPlane = 400;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float2 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float3 Normal : TEXCOORD1;
	float4 WorldPosition : TEXCOORD2;
	float2 Depth : TEXCOORD3;
	float2 UV : TEXCOORD4;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.WorldPosition = worldPosition;
	output.Normal = mul(input.Normal, World);
	output.Depth = output.Position.ww;
	output.UV = input.UV;
	return output;
}

struct PixelShaderOutput
{ 
	float4 Normal : COLOR0;  
	float4 Depth : COLOR1; 
	float4 Texture : COLOR2;
};

PixelShaderOutput PixelShaderFunction(VertexShaderOutput input)
{
	PixelShaderOutput output;
	float3 normal = normalize(input.Normal);
	normal /= 2;
	normal += 0.5;

	output.Normal = float4(normal, 1);
	output.Depth = float4(input.Depth.y / FarPlane, 1, 1, 1);
	
	output.Texture = float4(input.UV, 1, 1);
	

	return output;
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

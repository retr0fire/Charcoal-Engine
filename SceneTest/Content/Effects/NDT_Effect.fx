float4x4 World;
float4x4 View;
float4x4 Projection;

float3 DiffuseColor;
float Alpha = 1.0;
bool AlphaEnabled = false;
bool TextureEnabled;
texture BasicTexture;
sampler BasicTextureSampler = sampler_state {
	texture = <BasicTexture>;

};
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
	output.Texture = float4(1, 1, 1, Alpha);
	if (TextureEnabled == true)
		output.Texture = float4(tex2D(BasicTextureSampler, input.UV).xyz, Alpha);
	output.Texture *= float4(DiffuseColor, 1);
	if (tex2D(BasicTextureSampler, input.UV).a == 0)
	{
		if (AlphaEnabled)
		{
			output.Texture = float4(0, 0, 0, 0);
			output.Normal = float4(0, 0, 0, 0);
			discard;
		}
	}

	if (DiffuseColor.x == DiffuseColor.y == DiffuseColor.z == 0)
		DiffuseColor = float3(1, 1, 1);

	if (Alpha == 0)
	{
		if (AlphaEnabled)
		{
			output.Texture = float4(0, 0, 0, 0);
			output.Normal = float4(0, 0, 0, 0);
			discard;
		}
	}
	
	return output;
}

technique Technique1
{
	pass Pass1
	{
		// TODO: set renderstates here.

		VertexShader = compile vs_4_0_level_9_3 VertexShaderFunction();
		PixelShader = compile ps_4_0_level_9_3 PixelShaderFunction();
	}
}

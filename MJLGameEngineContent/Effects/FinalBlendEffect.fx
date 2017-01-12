float4x4 World;
float4x4 View;
float4x4 Projection;
float EnableLightMap=true;
float Alpha;
float3 TintColor = float3(1, 1, 1);
float NumberOfLights;
bool TextureEnabled;
texture BasicTexture;
sampler BasicTextureSampler = sampler_state {
texture = <BasicTexture>;
};

texture LightMap;
sampler LightMapSampler = sampler_state {
texture = <LightMap>;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float4 WorldPosition : TEXCOORD3;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	output.UV = input.UV;
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.WorldPosition = output.Position;
    // TODO: add your vertex shader code here.

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float2 tcoords;
	tcoords[0] = input.WorldPosition.x/input.WorldPosition.w/2.0f +0.5f;
	tcoords[1] = -input.WorldPosition.y/input.WorldPosition.w/2.0f +0.5f;
	float4 tv = tex2D(BasicTextureSampler, input.UV).rgba;
	if (TextureEnabled == false)
		tv = float4(1, 1, 1, 1);
	if (EnableLightMap == false)
	{
		return tv * Alpha * float4(TintColor,1);
	}
    return tv * tex2D(LightMapSampler, tcoords).rgba * float4(TintColor,1) * Alpha;
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

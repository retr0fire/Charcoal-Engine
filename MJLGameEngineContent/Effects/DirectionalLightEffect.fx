float4x4 World;
float4x4 View;
float4x4 Projection;

float4x4 WorldInverseTranspose;

float3 AmbientLightColor = float3(0, 0, 0);
float3 DiffuseColor = float3(.85, .85, .85);
float3 LightPosition = float3(0, 0, 0);
float3 LightColor = float3(1, 1, 1);
float LightAttenuation = 5000;
float LightFalloff = 2;

float3 CameraPosition = float3(0, 1, 10);

float SpecularPower = 20;

float Alpha = 1;

bool dds_Normal = false;
float bump_Height = 2;


bool TextureEnabled;
texture BasicTexture;
sampler BasicTextureSampler = sampler_state {
texture = <BasicTexture>;
};

bool NormalMapEnabled;
texture NormalMap;
sampler NormalMapSampler = sampler_state {
texture = <NormalMap>;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : NORMAL0;
    float3 Tangent : TANGENT0;
    float3 Binormal : BINORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float4 WorldPosition : TEXCOORD2;
	
    float3 Tangent : TEXCOORD3;
    float3 Binormal : TEXCOORD4;
	float3 ViewDirection : TEXCOORD5;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.WorldPosition = worldPosition;

	output.UV = input.UV;
	output.Normal = mul(input.Normal, World);
	
	output.Tangent = normalize(mul(input.Tangent, WorldInverseTranspose));
    output.Binormal = normalize(mul(input.Binormal, WorldInverseTranspose));

	output.ViewDirection = CameraPosition - worldPosition;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	// Calculate the normal, including the information in the bump map
	float4 bumptex = tex2D(NormalMapSampler, input.UV);
	float3 bump = float3(0,0,0);
	if (dds_Normal == true)
		bump = (bump_Height) * (bumptex.agb - (0.5, 0.5, 0.5));
	else
		bump = (bump_Height) * (bumptex - (0.5, 0.5, 0.5));
    float3 bumpNormal = normalize(input.Normal) + (bump.x * input.Tangent + bump.y * input.Binormal);
    bumpNormal = normalize(bumpNormal);

	float3 normal = normalize(input.Normal);
	if (NormalMapEnabled == true)
		normal = bumpNormal;

	float3 diffuseColor = DiffuseColor;
	float3 totalLight = float3(0, 0, 0);
	totalLight += AmbientLightColor;
	float3 lightDir = normalize(LightPosition - input.WorldPosition);
	float diffuse = saturate(dot(normal, lightDir));
	float d = distance(LightPosition, input.WorldPosition);
	float att = 1 - pow(clamp(d / LightAttenuation, 0, 1), LightFalloff);
	totalLight += diffuse * att * LightColor;

	float3 refl = reflect(lightDir, normal);
	float3 view = -normalize(input.ViewDirection);
	// Add specular highlights
	totalLight += (pow(saturate(dot(refl, view)), SpecularPower) *
	LightColor) * att;

	float3 final = diffuseColor * totalLight;
	return float4(final, 1);
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

float4x4 World;
float4x4 View;
float4x4 Projection;

float4x4 WorldInverseTranspose;

float3 AmbientLightColor = float3(0, 0, 0);
float3 DiffuseColor = float3(1, 1, 1);
float3 LightPosition = float3(0, 10, 0);
float3 LightDirection = float3(0, -1, 0);
float ConeAngle = 90;
float3 LightColor = float3(1, 1, 1);
float LightFalloff = 20;
float offset = 0.003;
float NearPlane;
float FarPlane;
float4x4 LightViewProjection;

float3 CameraPosition = float3(0, 1, 10);

float SpecularPower = 20;

bool dds_Normal = false;
float bump_Height = 2;

texture ShadowMap;
sampler ShadowMapSampler = sampler_state {
texture = <ShadowMap>;
 AddressU = clamp;
 AddressV = clamp;
 magfilter = POINT; 
 minfilter = POINT; 
 mipfilter=POINT; 
};

texture AlphaMap;
sampler AlphaMapSampler = sampler_state {
texture = <AlphaMap>;
 magfilter = LINEAR; 
 minfilter = LINEAR; 
 mipfilter=LINEAR; 
 AddressU = clamp;
 AddressV = clamp;
};

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
	float4 LightPosition :TEXCOORD3;
	
    float3 Tangent : TEXCOORD4;
    float3 Binormal : TEXCOORD5;
	float3 ViewDirection : TEXCOORD6;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	output.LightPosition = mul(input.Position, LightViewProjection);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.WorldPosition = worldPosition;

	output.UV = input.UV;
	output.Normal = input.Normal;
    output.Tangent = input.Tangent;
    output.Binormal = input.Binormal;
	
	output.ViewDirection = CameraPosition - worldPosition;


	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	input.Normal = mul(input.Normal, World);

	input.Tangent = normalize(mul(input.Tangent, WorldInverseTranspose));
    input.Binormal = normalize(mul(input.Binormal, WorldInverseTranspose));
	
	input.ViewDirection = CameraPosition - input.WorldPosition;



	// Calculate the normal, including the information in the bump map
	float4 bumptex = tex2D(NormalMapSampler, input.UV);

	float3 bump = float3(0,0,0);
    if (dds_Normal == true)
		bump = (bump_Height) * (bumptex.agb - (0.5, 0.5, 0.5));
	else
		bump = (bump_Height) * (bumptex - (0.5, 0.5, 0.5));
    float3 bumpNormal = normalize(input.Normal) + (bump.x * input.Tangent + bump.y * input.Binormal);
    bumpNormal = normalize(bumpNormal);
	
	float2 tcoords;
	tcoords[0] = input.LightPosition.x/input.LightPosition.w/2.0f +0.5f;
	tcoords[1] = -input.LightPosition.y/input.LightPosition.w/2.0f +0.5f;
	
	float d = distance(input.LightPosition, float3(0, 0, 0));
	d -= NearPlane;
	d /= (FarPlane-NearPlane);

	float shadow = tex2D(ShadowMapSampler, tcoords);

	if (tcoords[1] > 1)
		return float4(AmbientLightColor, 1);

	if (tcoords[1] < -1)
		return float4(AmbientLightColor, 1);

	if (tcoords[0] > 1)
		return float4(AmbientLightColor, 1);

	if (tcoords[0] < -1)
		return float4(AmbientLightColor, 1);

	if (d < (shadow+offset))
	{
		float3 normal = normalize(input.Normal);
		if (NormalMapEnabled == true)
			normal = bumpNormal;
		float adds = 0;
		float3 diffuseColor = DiffuseColor;
		float3 totalLight = AmbientLightColor;
		float3 lightDir = normalize(LightPosition - input.WorldPosition);
		float diffuse = saturate(dot(normal, lightDir));
		float d = dot(-lightDir, normalize(LightDirection));
		float a = cos(ConeAngle);
		float att = 0;
		if (a < d)
		{
			att = 1 - pow(clamp(a / d, 0, 1), LightFalloff);
			adds = 1;
		}
		totalLight += diffuse * att * LightColor;
		
		float3 refl = reflect(lightDir, normal);
		float3 view = -normalize(input.ViewDirection);
		// Add specular highlights
		if (adds == 1)
		{
		totalLight += (pow(saturate(dot(refl, view)), SpecularPower) *
			att);
		}
		float3 final = diffuseColor * totalLight * (1-(tex2D(AlphaMapSampler, tcoords).r));

		if (final.r*final.g*final.b < AmbientLightColor.r*AmbientLightColor.g*AmbientLightColor.b)
			final = AmbientLightColor;
		return float4(final, 1);
	}
	return float4(AmbientLightColor, 1);
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}

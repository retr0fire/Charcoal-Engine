float4x4 WorldViewProjection; 
float4x4 inverse_ViewProjection;
float4x4 ViewProjection;
float4x4 inverse_view;
float4x4 view;
float4x4 inverse_world;
float4x4 world;
float4x4 inverse_projection;
float4x4 projection;



texture2D DepthTexture; 
texture2D NormalTexture; 

float3 LightColor; 
float3 LightPosition; 
float LightAttenuation;

//=
float2 TanAspect;
float FarPlane;

float3 cam_pos;

float viewportWidth; 
float viewportHeight;


float3 DiffuseColor;
bool TextureEnabled;
texture BasicTexture;
sampler BasicTextureSampler = sampler_state {
	texture = <BasicTexture>;
};
sampler2D depthSampler = sampler_state { 
	texture = <DepthTexture>;   
	minfilter = point;  
	magfilter = point;  
	mipfilter = point; 
}; 

sampler2D normalSampler = sampler_state { 
	texture = <NormalTexture>;   
	minfilter = point;  
	magfilter = point;  
	mipfilter = point; 
};

struct VertexShaderInput
{ 
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
};


float4 PixelShaderFunction(float4 Position : SV_POSITION, float4 color : COLOR0, float2 UV : TEXCOORD0) : COLOR0
{ 
	float2 texCoord = UV;
	// Extract the depth for this pixel from the depth map  
	float depth = tex2D(depthSampler, texCoord).r;
	depth *= FarPlane;
	float3 pos = float3((TanAspect * ((texCoord * 2)-float2(1,1)) * depth), -depth);
	pos = mul(pos, inverse_view);
	pos += cam_pos;
	
	//return float4(pos, 1);


	float4 normal = (tex2D(normalSampler, texCoord) - .5) * 2;     
	// Perform the lighting calculations for a point light 
	float3 lightDirection = normalize(LightPosition - pos);  
	float lighting = clamp(dot(normal, lightDirection), 0, 1);     
	// Attenuate the light to simulate a point light  
	float d = distance(LightPosition, pos);  
	float att = 1 - d / LightAttenuation;// , 2);
	if (att < 0) att = 0;
	if (lighting < 0) lighting = 0;
	if (depth == FarPlane)
		return float4(0, 0, 0, 0);
	return float4(LightColor * lighting * att, 1) + float4(.3, .3, .3, 0);
}

technique Technique1
{ 
	pass Pass1
	{ 
		PixelShader = compile ps_4_0_level_9_3 PixelShaderFunction(); 
	} 
}
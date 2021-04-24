float4x4 ViewProjection;
float4x4 InverseViewProjection;
float4x4 World;
float3 Position;
float w;
float h;
float3 CameraPosition;
float3 gradients[2*2];
float NearClip;
float FarClip;
float Radius;

struct VertexShaderInput
{
	float4 Position : POSITION0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
};

float SphereSDF(float3 SpherePosition, float3 TestPosition, float Radius)
{
	return distance(SpherePosition, TestPosition) - Radius;

}
float Fade(float t)
{
	// Fade function as defined by Ken Perlin.  This eases coordinate values
	// so that they will ease towards integral values.  This ends up smoothing
	// the final output.
	return t * t * t * (t * (t * 6 - 15) + 10);         // 6t^5 - 15t^4 + 10t^3
}

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	output.Position = input.Position;

	output.Position.z = 1.0;
	
	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float posx = (2 * input.Position.x / w - 1);
	float posy = -(2 * input.Position.y / h - 1);
	float2 ScreenPosition = float2(posx, posy);

	float4 PreUnProject = float4(ScreenPosition.x, ScreenPosition.y, 0.0, 1);
	float4 WorldPosition = mul(PreUnProject, InverseViewProjection);

	float4 PreUnProjectCenter = float4(0, 0, 0.0, 1);
	float4 WorldPositionCenter = mul(PreUnProjectCenter, InverseViewProjection);

	float4 SpherePosition = mul(float4(Position, 1), World);
	SpherePosition = SpherePosition / SpherePosition.w;
	
	WorldPosition = WorldPosition / WorldPosition.w;
	WorldPositionCenter = WorldPositionCenter / WorldPositionCenter.w;

	float3 Ray = normalize(WorldPosition.xyz - CameraPosition);
	float3 RayCenter = normalize(WorldPositionCenter.xyz - CameraPosition);
	
	//float3 Dir = normalize(SpherePosition.xyz - CameraPosition);
	
	//float Env = normalize(float2(length(SpherePosition.xyz - CameraPosition), Radius)).x;
	
	//if (dot(Ray, Dir) > Env)
	//	return float4(dot(Ray, Dir), 0, 0, 1);
	//return float4(0, 0, 0, 1);
	
	float3 MarchPos = CameraPosition;
	
	for (int i = 0; i < 10; i++)
	{
		float march_dist = SphereSDF(SpherePosition.xyz, MarchPos, Radius);
		if (march_dist < 0.01f)
		{
			//float4(1, 0, 0, 1);//
			//return float4(distance(MarchPos, CameraPosition), 0, 0, 1);
            //bad lighting:
            float3 light_dir = float3(1, -1, 0);
            
            float3 normal = normalize(MarchPos - SpherePosition.xyz);
            
            float light = clamp(dot(-light_dir, normal), 0.0f, 2.0f)/2;
            float3 lcolor = float3(1, 1, 1);
            float3 scolor = float3(0.6, 0.6, 0.6);
            return float4(scolor * light * lcolor, 1);

        }
		MarchPos += Ray * march_dist;
	}

	return float4(0, 0, 0, 1);
	
	//ray * dir must be less than env * dir
	
	/*float2 gradients[2][2];
	gradients[0][0] = normalize(float2(1, 0.3));
	gradients[1][0] = normalize(float2(1, -1));
	gradients[1][1] = normalize(float2(-0.1, 1));
	gradients[0][1] = normalize(float2(-0.2, -1));*/

	/*float2 corner_spacing = float2(2.0, 2.0);
	float3 corners[2][2];
	corners[0][0] = float3(-1.0, -1.0, 0.0);
	corners[1][0] = float3(1.0, -1.0, 0.0);
	corners[1][1] = float3(1.0, 1.0, 0.0);
	corners[0][1] = float3(-1.0, 1.0, 0.0);

	float2 distance[2][2];
	distance[0][0] = (Position - corners[0][0]) / corner_spacing;
	distance[1][0] = (Position - corners[1][0]) / corner_spacing;
	distance[1][1] = (Position - corners[1][1]) / corner_spacing;
	distance[0][1] = (Position - corners[0][1]) / corner_spacing;*/

	//float result = lerp(
	//	lerp(dot(distance[0][0], gradients[0][0]), dot(distance[1][0], gradients[1][0]), Fade((Position.x - corners[0][0].x) / corner_spacing)),
	//	lerp(dot(distance[0][1], gradients[0][1]), dot(distance[1][1], gradients[1][1]), Fade((Position.x - corners[0][0].x) / corner_spacing)),
	//	Fade((Position.y - corners[0][0].y) / corner_spacing)
	//);

	/*float x1 = lerp(dot(distance[0][0], gradients[0 * 2 + 0]), dot(distance[1][0], gradients[1 * 2 + 0]), Fade((Position.x - corners[0][0].x) / corner_spacing));
	float x2 = lerp(dot(distance[0][1], gradients[0 * 2 + 1]), dot(distance[1][1], gradients[1 * 2 + 1]), Fade((Position.x - corners[0][0].x) / corner_spacing));

	float result = lerp(x1, x2, Fade((Position.y - corners[0][0].y) / corner_spacing));

	result = result * 2;
	
	float k = 0.0;
	if (abs(result) > 0.8)
		k = 1;

	return saturate(float4(0.8, 0.4, 1, 1)*result) + saturate(float4(0.3, 0.3, 1, 1)*(-result));
	*/

	/*float4 ProjPosition = mul(float4(position, 1.0), WorldViewProjection);

	ProjPosition = ProjPosition / ProjPosition.w;

	float posx = (2 * input.Position.x / w - 1);
	float posy = -(2 * input.Position.y / h - 1);

	float px = ProjPosition.x;
	float py = ProjPosition.y;

	float3 dist = float3(px - posx, py - posy, 0);

	if (length(dist) < 0.1f)
		return float4(0, 1, 1, 1);

	return float4(length(dist), 0, 0, 1);*/



	}

technique Specular
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 VertexShaderFunction();
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}
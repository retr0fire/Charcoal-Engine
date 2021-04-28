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

#define MAX_ENTITIES  15

float RadiusArray[MAX_ENTITIES];
float4x4 WorldArray[MAX_ENTITIES];
int ActiveEntities;

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

float UnionSDF(float a, float b)
{
    return min(a, b);
}

float Map(float3 Position)
{
    float march_dist = 100000;
    for (int s = 0; s < ActiveEntities; s++)
    {
        float4 SpherePosition = mul(float4(float3(0, 0, 0), 1), WorldArray[s]);
        SpherePosition = SpherePosition / SpherePosition.w;
        
        float test_dist = SphereSDF(SpherePosition.xyz, Position, RadiusArray[s]);
        
        march_dist = UnionSDF(test_dist, march_dist);
    }
    
    return march_dist;
}

float3 GetNormal(float3 pos)
{
    // epsilon - used to approximate dx when taking the derivative
    const float2 eps = float2(0.01, 0.0);

    // The idea here is to find the "gradient" of the distance field at pos
    // Remember, the distance field is not boolean - even if you are inside an object
    // the number is negative, so this calculation still works.
    // Essentially you are approximating the derivative of the distance field at this point.
    float3 nor = float3(
        Map(pos + eps.xyy).x - Map(pos - eps.xyy).x,
        Map(pos + eps.yxy).x - Map(pos - eps.yxy).x,
        Map(pos + eps.yyx).x - Map(pos - eps.yyx).x);
    return normalize(nor);
}

float MarchToSurface(float3 MarchPos, float3 Ray)
{
    float3 StartPos = MarchPos;
    for (int i = 0; i < 50; i++)
    {
        float march_dist = Map(MarchPos);
            
        if (march_dist < 0.1f)
        {
            return distance(StartPos, MarchPos);
        }
        MarchPos += Ray * march_dist;
    }
    return -1;
}

float3 Colorize(float3 Normal, float3 LightDirection, float3 LightColor, float3 SurfaceColor)
{
    float light = clamp(dot(-LightDirection, Normal), 0.0f, 1.0f);
    return float4(SurfaceColor * light * LightColor, 1);
}

float3 Atmosphere(float3 Ray)
{
    return float3(0.0, 0.1, Ray.y + 0.3);
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
    //March once
    float MarchDist = MarchToSurface(MarchPos, Ray);
    
    if (MarchDist < 0.0f)
        return float4(Atmosphere(Ray), 1);
    
    MarchPos = MarchPos + Ray * MarchDist;
    float3 light_dir = normalize(float3(1, -1, 0));
    float3 Normal = GetNormal(MarchPos);
    float3 color = float3(0.2, 0.2, 0.2);//Colorize(Normal, light_dir, float3(1, 1, 1), float3(1, 1, 0.5));
    Ray = Ray + Normal * 2;
    //Find new march position
    MarchDist = MarchToSurface(MarchPos + (Ray + Normal * 2)/10.0f, Ray + Normal * 2);
    
    if (MarchDist < 0.0f)
        return float4(color + Atmosphere(Ray), 1);
    
    MarchPos = MarchPos + Ray * MarchDist;
    Normal = GetNormal(MarchPos);
    Ray = Ray + Normal * 2;
    color += float3(0.2, 0.2, 0.2) + Atmosphere(Ray); //Colorize(Normal, light_dir, float3(1, 1, 1), float3(1, 1, 0.5));
    
	return float4(color, 1);
	
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
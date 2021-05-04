float4x4 ViewProjection;
float4x4 InverseViewProjection;
float4x4 World;
float4x4 InverseWorld;
float3 CameraPosition;
float3 CornerMin;
float3 CornerMax;
float NearClip;
float FarClip;
float w;
float h;


float DistanceToPlane(float3 P, float K, float3 Ray, float3 Origin)
{
    return (K - dot(Origin, P)) / (dot(Ray, P));
}

float DistanceToBox(float3 Min, float3 Max, float3 Position, float3 Ray)
{
    float XPlaneMin = DistanceToPlane(float3(1, 0, 0), Min.x, Ray, Position);
    float XPlaneMax = DistanceToPlane(float3(1, 0, 0), Max.x, Ray, Position);
    float YPlaneMin = DistanceToPlane(float3(0, 1, 0), Min.y, Ray, Position);
    float YPlaneMax = DistanceToPlane(float3(0, 1, 0), Max.y, Ray, Position);
    float ZPlaneMin = DistanceToPlane(float3(0, 0, 1), Min.z, Ray, Position);
    float ZPlaneMax = DistanceToPlane(float3(0, 0, 1), Max.z, Ray, Position);
    
    if (XPlaneMin < 0.01)
        XPlaneMin = FarClip;
    if (XPlaneMax < 0.01)
        XPlaneMax = FarClip;
    
    if (YPlaneMin < 0.01)
        YPlaneMin = FarClip;
    if (YPlaneMax < 0.01)
        YPlaneMax = FarClip;
    
    if (ZPlaneMin < 0.01)
        ZPlaneMin = FarClip;
    if (ZPlaneMax < 0.01)
        ZPlaneMax = FarClip;
    
    float3 XPlaneMinPoint = Position + Ray * XPlaneMin;
    float3 XPlaneMaxPoint = Position + Ray * XPlaneMax;
    float3 YPlaneMinPoint = Position + Ray * YPlaneMin;
    float3 YPlaneMaxPoint = Position + Ray * YPlaneMax;
    float3 ZPlaneMinPoint = Position + Ray * ZPlaneMin;
    float3 ZPlaneMaxPoint = Position + Ray * ZPlaneMax;
    
    if (XPlaneMinPoint.y > CornerMax.y || XPlaneMinPoint.y < CornerMin.y || XPlaneMinPoint.z > CornerMax.z || XPlaneMinPoint.z < CornerMin.z)
    {
        XPlaneMin = FarClip;
    }
    if (XPlaneMaxPoint.y > CornerMax.y || XPlaneMaxPoint.y < CornerMin.y || XPlaneMaxPoint.z > CornerMax.z || XPlaneMaxPoint.z < CornerMin.z)
    {
        XPlaneMax = FarClip;
    }
    
    if (YPlaneMinPoint.x > CornerMax.x || YPlaneMinPoint.x < CornerMin.x || YPlaneMinPoint.z > CornerMax.z || YPlaneMinPoint.z < CornerMin.z)
    {
        YPlaneMin = FarClip;
    }
    if (YPlaneMaxPoint.x > CornerMax.x || YPlaneMaxPoint.x < CornerMin.x || YPlaneMaxPoint.z > CornerMax.z || YPlaneMaxPoint.z < CornerMin.z)
    {
        YPlaneMax = FarClip;
    }
    
    if (ZPlaneMinPoint.x > CornerMax.x || ZPlaneMinPoint.x < CornerMin.x || ZPlaneMinPoint.y > CornerMax.y || ZPlaneMinPoint.y < CornerMin.y)
    {
        ZPlaneMin = FarClip;
    }
    if (ZPlaneMaxPoint.x > CornerMax.x || ZPlaneMaxPoint.x < CornerMin.x || ZPlaneMaxPoint.y > CornerMax.y || ZPlaneMaxPoint.y < CornerMin.y)
    {
        ZPlaneMax = FarClip;
    }
    
    return min(min(ZPlaneMin, ZPlaneMax), min(min(XPlaneMin, XPlaneMax), min(YPlaneMin, YPlaneMax)));
}

struct VertexShaderInput
{
	float4 Position : POSITION0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
};

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

	float4 PreUnProjectCenter = float4(0, 0, 0, 1);
	float4 WorldPositionCenter = mul(PreUnProjectCenter, InverseViewProjection);

    WorldPosition = WorldPosition / WorldPosition.w;
	WorldPositionCenter = WorldPositionCenter / WorldPositionCenter.w;

	float3 Ray = normalize(WorldPosition.xyz - CameraPosition);
	float3 RayCenter = normalize(WorldPositionCenter.xyz - CameraPosition);
	
	float3 MarchPos = CameraPosition;
    
    float dist = DistanceToBox(CornerMin, CornerMax, CameraPosition, Ray);
    
    if (dist == FarClip)
    {
        return float4(0, 0, 0, 1);
    }
    
    return float4(dist/10, 0, 0, 1);
}

technique Specular
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 VertexShaderFunction();
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}
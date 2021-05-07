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

/*

texture DensityMap;
sampler DensityMapSampler = sampler_state {
texture = <DensityMap>;
 AddressU = clamp;
 AddressV = clamp;
 magfilter = POINT; 
 minfilter = POINT; 
 mipfilter=POINT; 
};

*/

float3 BackgroundColor;

float t;


float DistanceToPlane(float3 P, float K, float3 Ray, float3 Origin)
{
    return (K - dot(Origin, P)) / (dot(Ray, P));
}

float DistanceToBox(float3 Min, float3 Max, float3 Position, float3 Ray)
{
    //if position is already in the box, return 0
    if (Position.x < CornerMax.x && Position.x > CornerMin.x && Position.y < CornerMax.y && Position.y > CornerMin.y && Position.z < CornerMax.z && Position.z > CornerMin.z)
    {
        return 0;
    }
    
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
/*

int3 GetVoxelIndices(float3 CMin, float3 CMax, float3 Position, float VoxelsPerUnit)
{
    return int3(round((Position - CMin) / (CMax - CMin)));
}

float3 GetDensityAtVoxel(int3 Voxel, int Granularity)
{
    return tex2D(DensityMapSampler, Voxel.xy / (float) Granularity);
}*/


int3 GetVoxelIndices(float3 CMin, float3 Position, float VoxelsPerUnit)
{
    float3 Pos = Position - CMin;
    
    Pos *= VoxelsPerUnit;
    
    return int3(round(Pos));
}

float GetDensityAtVoxel(int3 Voxel, int Granularity)
{
    float3 VoxelCenter = float3(1, 1, 1) * Granularity;
    
    float3 FVoxel = float3(sin(t), sin(t), sin(t)) * (Voxel - VoxelCenter) + VoxelCenter;
    
    float radius = length(FVoxel - VoxelCenter);
    
    return min(0.1 / radius, 0.003);

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
    
    float4 CMin = mul(float4(CornerMin, 1), World);
    CornerMin = (CMin / CMin.w).xyz;
    float4 CMax = mul(float4(CornerMax, 1), World);
    CornerMax = (CMax / CMax.w).xyz;
    
    float dist = DistanceToBox(CornerMin, CornerMax, CameraPosition, Ray);
    
    if (dist == FarClip)
    {
        return float4(BackgroundColor, 1);
    }
    
    float3 InterSectionPoint = CameraPosition + Ray * dist;
    
    float3 AlterStartPoint = CameraPosition + Ray * dist + Ray * distance(CornerMax, CornerMin);
    float alter_dist = DistanceToBox(CornerMin, CornerMax, AlterStartPoint, -Ray);
    float3 AlterInterSectionPoint = AlterStartPoint + (-Ray) * alter_dist;
    
    //accessing the texture
    //float3 internalposition = InterSectionPoint - CornerMin;
    
    float granularity = 100;
    
    float intensity = 0.0f;// = distance(AlterInterSectionPoint, InterSectionPoint) / 5;
    int steps = trunc(distance(InterSectionPoint, AlterInterSectionPoint) * granularity);
    float partial_step = distance(InterSectionPoint, AlterInterSectionPoint) * granularity - steps;
    float distance_through_box = distance(InterSectionPoint, AlterInterSectionPoint);
    float step_length = distance_through_box / (float) steps;
    float3 position_in_box = InterSectionPoint;
    for (int i = 0; i < steps; i++)
    {
        position_in_box += Ray * step_length;
        int3 voxel = GetVoxelIndices(CornerMin, position_in_box, granularity);
        intensity += GetDensityAtVoxel(voxel, granularity);
        
        if (i == steps - 1)
        {
            //last step
            intensity += GetDensityAtVoxel(voxel, granularity) * partial_step;
        }

    }
    
    return float4(BackgroundColor + float3(1, 0.5, 1) * intensity, 1);
}

technique Specular
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 VertexShaderFunction();
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}
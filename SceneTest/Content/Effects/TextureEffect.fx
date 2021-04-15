float4x4 WorldViewProjection;
float3 position;
float w;
float h;

float2 gradients[2*2];

struct VertexShaderInput
{
	float4 Position : POSITION0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
};

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
	float2 Position = float2(posx, posy);

	/*float2 gradients[2][2];
	gradients[0][0] = normalize(float2(1, 0.3));
	gradients[1][0] = normalize(float2(1, -1));
	gradients[1][1] = normalize(float2(-0.1, 1));
	gradients[0][1] = normalize(float2(-0.2, -1));*/

	float2 corner_spacing = float2(2.0, 2.0);
	float2 corners[2][2];
	corners[0][0] = float2(-1.0, -1.0);
	corners[1][0] = float2(1.0, -1.0);
	corners[1][1] = float2(1.0, 1.0);
	corners[0][1] = float2(-1.0, 1.0);

	float2 distance[2][2];
	distance[0][0] = (Position - corners[0][0]) / corner_spacing;
	distance[1][0] = (Position - corners[1][0]) / corner_spacing;
	distance[1][1] = (Position - corners[1][1]) / corner_spacing;
	distance[0][1] = (Position - corners[0][1]) / corner_spacing;

	//float result = lerp(
	//	lerp(dot(distance[0][0], gradients[0][0]), dot(distance[1][0], gradients[1][0]), Fade((Position.x - corners[0][0].x) / corner_spacing)),
	//	lerp(dot(distance[0][1], gradients[0][1]), dot(distance[1][1], gradients[1][1]), Fade((Position.x - corners[0][0].x) / corner_spacing)),
	//	Fade((Position.y - corners[0][0].y) / corner_spacing)
	//);

	float x1 = lerp(dot(distance[0][0], gradients[0 * 2 + 0]), dot(distance[1][0], gradients[1 * 2 + 0]), Fade((Position.x - corners[0][0].x) / corner_spacing));
	float x2 = lerp(dot(distance[0][1], gradients[0 * 2 + 1]), dot(distance[1][1], gradients[1 * 2 + 1]), Fade((Position.x - corners[0][0].x) / corner_spacing));

	float result = lerp(x1, x2, Fade((Position.y - corners[0][0].y) / corner_spacing));

	result = result * 2;
	//result = result + 1;

	float k = 0.0;
	if (abs(result) > 0.8)
		k = 1;

	return saturate(float4(0.8, 0.4, 1, 1)*result) + saturate(float4(0.3, 0.3, 1, 1)*(-result));


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
float4x4 World;
float4x4 View;
float4x4 Projection;
float3 position;
float w;
float h;

struct VertexShaderInput
{
	float4 Position : POSITION0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Point : POSITION1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;


	output.Position = input.Position;

	output.Position.z = 1.0;

	float4 worldPosition = mul(float4(position, 1.0), World);
	float4 viewPosition = mul(worldPosition, View);
	output.Point = mul(viewPosition, Projection);
	
	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	input.Point = input.Point / input.Point.w;

	float posx = (2 * input.Position.x / w - 1);
	float posy = -(2 * input.Position.y / h - 1);

	float px = input.Point.x;// / (w / 1000);
	float py = input.Point.y;// / (h / 1000);

	float3 dist = float3(px - posx, py - posy, 0);

	return float4(length(dist), 0, 0, 1);
}

technique Specular
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 VertexShaderFunction();
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}
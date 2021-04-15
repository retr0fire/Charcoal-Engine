float4x4 WorldViewProjection;
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
	float4 ProjPosition = mul(float4(position, 1.0), WorldViewProjection);

	ProjPosition = ProjPosition / ProjPosition.w;

	float posx = (2 * input.Position.x / w - 1);
	float posy = -(2 * input.Position.y / h - 1);

	float px = ProjPosition.x;// / (w / 1000);
	float py = ProjPosition.y;// / (h / 1000);

	float3 dist = float3(px - posx, py - posy, 0);

	if (length(dist) < 0.1f)
		return float4(0, 1, 1, 1);

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
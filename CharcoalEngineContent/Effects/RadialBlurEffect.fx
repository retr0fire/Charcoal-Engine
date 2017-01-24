// The texture to blur
texture ScreenTexture;

sampler2D tex = sampler_state {
	texture = <ScreenTexture>;
	minfilter = point;
	magfilter = point;
	mipfilter = point;
};

// Precalculated weights and offsets
float weights[15] = { 0.1061154, 0.1028506, 0.1028506, 0.09364651,
0.09364651, 0.0801001, 0.0801001, 0.06436224, 0.06436224,
0.04858317, 0.04858317, 0.03445063, 0.03445063, 0.02294906,
0.02294906 };

float offsets[15] = { 0, 0.00125, -0.00125, 0.002916667,
-0.002916667, 0.004583334, -0.004583334, 0.00625, -0.00625,
0.007916667, -0.007916667, 0.009583334, -0.009583334, 0.01125,
-0.01125 };

// Blurs the input image horizontally
float4 BlurHorizontal(float4 Position : POSITION0, float2 UV : TEXCOORD0) : COLOR0
{
	float4 output = float4(0, 0, 0, 1);
	// Sample from the surrounding pixels using the precalculated
	// pixel offsets and color weights
	float2 UVM = UV*2-1;
	for (int i = 0; i < 15; i++)
	{
		output += tex2D(tex, UV + float2(offsets[i] * UVM.x, offsets[i] * UVM.y)) * weights[i];
		
	}
	return lerp(tex2D(tex, UV), output, min(length(UVM) + 0.3, 1.0));
}

technique Technique1
{
	pass Horizontal
	{
		PixelShader = compile ps_2_0 BlurHorizontal();
	}
}
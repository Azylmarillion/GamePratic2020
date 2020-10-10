// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FireFurnace"
{
	Properties
	{
		_Noise2scale("Noise 2 scale", Float) = 1.5
		_Noise2speed("Noise 2 speed", Float) = -1
		_Noise1scale("Noise 1 scale", Float) = 1.5
		_Noise1speed("Noise 1 speed", Float) = -0.7
		_Texture0("Texture 0", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_Innerflamestep("Inner flame step", Range( 0.0001 , 0.1)) = 0.03116814
		_shadingamount("shading amount", Range( 0 , 1)) = 0.7896312
		_Opacity("Opacity", Range( 0.0001 , 0.1)) = 0.01647059
		_Blendingcolor("Blending color", Color) = (1,0,0.1743317,0)
		_Outercolor("Outer color", Color) = (1,0.6766883,0,0)
		_Innercolor("Inner color", Color) = (1,0.9174029,0.5424528,0)
		_Outcolorblend("Out color blend", Range( 0 , 1)) = 1
		_Brightness("Brightness", Range( 0 , 5)) = 1.882353
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _shadingamount;
		uniform sampler2D _TextureSample3;
		SamplerState sampler_TextureSample3;
		uniform sampler2D _Texture0;
		uniform float _Noise1speed;
		uniform float _Noise1scale;
		uniform float _Noise2speed;
		uniform float _Noise2scale;
		uniform sampler2D _TextureSample2;
		SamplerState sampler_TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform float4 _Innercolor;
		uniform float _Innerflamestep;
		uniform float _Opacity;
		uniform float4 _Outercolor;
		uniform float _Outcolorblend;
		uniform float4 _Blendingcolor;
		uniform float _Brightness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 appendResult14 = (float2(0.0 , _Noise1speed));
			float2 uv_TexCoord3 = i.uv_texcoord * float2( 1.5,1 );
			float2 panner13 = ( 1.0 * _Time.y * appendResult14 + ( _Noise1scale * uv_TexCoord3 ));
			float2 appendResult19 = (float2(0.0 , _Noise2speed));
			float2 panner20 = ( 1.0 * _Time.y * appendResult19 + ( uv_TexCoord3 * _Noise2scale ));
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 tex2DNode46 = tex2D( _TextureSample2, uv_TextureSample2 );
			float2 appendResult43 = (float2(( ( ( tex2D( _Texture0, panner13 ).r * tex2D( _Texture0, panner20 ).r ) + tex2DNode46.r ) * tex2DNode46.r ) , 0.0));
			float4 tex2DNode44 = tex2D( _TextureSample3, appendResult43 );
			float temp_output_49_0 = step( _Innerflamestep , tex2DNode44.r );
			float temp_output_50_0 = step( _Opacity , tex2DNode44.r );
			float smoothstepResult77 = smoothstep( 0.0 , _Outcolorblend , i.uv_texcoord.y);
			o.Emission = ( saturate( ( ( 1.0 - _shadingamount ) + tex2DNode44.r ) ) * ( ( _Innercolor * temp_output_49_0 ) + ( ( temp_output_50_0 - temp_output_49_0 ) * ( ( _Outercolor * ( 1.0 - smoothstepResult77 ) ) + ( smoothstepResult77 * _Blendingcolor ) ) ) ) * _Brightness ).rgb;
			o.Alpha = temp_output_50_0;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
536;187;1255;691;-147.9927;9.857239;1;True;False
Node;AmplifyShaderEditor.Vector2Node;32;-1343.399,310.3037;Inherit;False;Constant;_Vector0;Vector 0;6;0;Create;True;0;0;False;0;False;1.5,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;11;-1105.008,188.2821;Inherit;False;Property;_Noise1scale;Noise 1 scale;2;0;Create;True;0;0;False;0;False;1.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1153.008,291.2821;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-1114.008,547.282;Inherit;False;Property;_Noise2speed;Noise 2 speed;1;0;Create;True;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1112.008,86.28204;Inherit;False;Property;_Noise1speed;Noise 1 speed;3;0;Create;True;0;0;False;0;False;-0.7;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1106.008,450.282;Inherit;False;Property;_Noise2scale;Noise 2 scale;0;0;Create;True;0;0;False;0;False;1.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-849.0082,383.282;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;19;-850.0082,514.282;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;14;-851.0082,132.2821;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-851.0082,258.2822;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;21;-682.0083,249.2821;Inherit;True;Property;_Texture0;Texture 0;4;0;Create;True;0;0;False;0;False;421e462846b502942942179c645345da;421e462846b502942942179c645345da;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;20;-636.0084,448.282;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;13;-633.0084,128.2821;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;25;-402.0082,131.2821;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;30;-398.585,356.3461;Inherit;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-79.58492,255.3461;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;46;-180.5686,550.8852;Inherit;True;Property;_TextureSample2;Texture Sample 2;5;0;Create;True;0;0;False;0;False;-1;3c4bbbc5e01964c4e83fad8ec4dcf53f;3c4bbbc5e01964c4e83fad8ec4dcf53f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;34;170.9107,252.2215;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;317.2586,251.9801;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;76;343.4864,753.184;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;78;275.4865,904.184;Inherit;False;Property;_Outcolorblend;Out color blend;13;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;43;473.944,252.6585;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SmoothstepOpNode;77;603.4861,800.184;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;641.6865,124.784;Inherit;False;Property;_Innerflamestep;Inner flame step;7;0;Create;True;0;0;False;0;False;0.03116814;0.17;0.0001;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;44;631.8108,222.7349;Inherit;True;Property;_TextureSample3;Texture Sample 3;6;0;Create;True;0;0;False;0;False;-1;3c4bbbc5e01964c4e83fad8ec4dcf53f;3c4bbbc5e01964c4e83fad8ec4dcf53f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;69;648.6865,426.784;Inherit;False;Property;_Opacity;Opacity;9;0;Create;True;0;0;False;0;False;0.01647059;0;0.0001;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;82;535.4863,1030.184;Inherit;False;Property;_Blendingcolor;Blending color;10;0;Create;True;0;0;False;0;False;1,0,0.1743317,0;1,0,0.1743317,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;79;793.4861,805.184;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;71;723.386,603.1837;Inherit;False;Property;_Outercolor;Outer color;11;0;Create;True;0;0;False;0;False;1,0.6766883,0,0;1,0.6766883,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;87;834.2172,-128.9597;Inherit;False;Property;_shadingamount;shading amount;8;0;Create;True;0;0;False;0;False;0.7896312;0.17;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;50;1014.687,332.784;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;797.4861,901.184;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;49;1015.687,192.784;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;983.686,776.6841;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;70;1089.687,-8.216003;Inherit;False;Property;_Innercolor;Inner color;12;0;Create;True;0;0;False;0;False;1,0.9174029,0.5424528,0;1,0.9174029,0.5424528,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;88;1118.917,-123.7597;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;85;1193.817,772.4404;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;74;1183.687,329.784;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;91;939.7169,14.84039;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;89;1280.117,-122.4596;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;1346.687,372.784;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;1347.687,171.784;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;92;1374.465,470.1035;Inherit;False;Property;_Brightness;Brightness;14;0;Create;True;0;0;False;0;False;1.882353;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;99;1185.805,614.2024;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;90;1416.617,-122.4596;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;73;1521.687,263.784;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;1708.617,240.7404;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RelayNode;101;1537.805,637.2024;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2019.809,191.5875;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;FireFurnace;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;32;0
WireConnection;6;0;3;0
WireConnection;6;1;9;0
WireConnection;19;1;10;0
WireConnection;14;1;12;0
WireConnection;4;0;11;0
WireConnection;4;1;3;0
WireConnection;20;0;6;0
WireConnection;20;2;19;0
WireConnection;13;0;4;0
WireConnection;13;2;14;0
WireConnection;25;0;21;0
WireConnection;25;1;13;0
WireConnection;30;0;21;0
WireConnection;30;1;20;0
WireConnection;31;0;25;1
WireConnection;31;1;30;1
WireConnection;34;0;31;0
WireConnection;34;1;46;1
WireConnection;41;0;34;0
WireConnection;41;1;46;1
WireConnection;43;0;41;0
WireConnection;77;0;76;2
WireConnection;77;2;78;0
WireConnection;44;1;43;0
WireConnection;79;0;77;0
WireConnection;50;0;69;0
WireConnection;50;1;44;1
WireConnection;80;0;77;0
WireConnection;80;1;82;0
WireConnection;49;0;67;0
WireConnection;49;1;44;1
WireConnection;83;0;71;0
WireConnection;83;1;79;0
WireConnection;88;0;87;0
WireConnection;85;0;83;0
WireConnection;85;1;80;0
WireConnection;74;0;50;0
WireConnection;74;1;49;0
WireConnection;91;0;44;1
WireConnection;89;0;88;0
WireConnection;89;1;91;0
WireConnection;75;0;74;0
WireConnection;75;1;85;0
WireConnection;72;0;70;0
WireConnection;72;1;49;0
WireConnection;99;0;50;0
WireConnection;90;0;89;0
WireConnection;73;0;72;0
WireConnection;73;1;75;0
WireConnection;86;0;90;0
WireConnection;86;1;73;0
WireConnection;86;2;92;0
WireConnection;101;0;99;0
WireConnection;0;2;86;0
WireConnection;0;9;101;0
ASEEND*/
//CHKSM=780994CC04A12FC24C785A96737B8F29EACA1909
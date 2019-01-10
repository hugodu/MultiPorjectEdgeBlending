// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Blending"
{
	Properties
	{
		_Texture0("Texture 0", 2D) = "white" {}
		_Tiling("Tiling", Vector) = (0.6,1,0,0)
		_Offset("Offset", Vector) = (0,1,0,0)
		_H_BlendDistance("H_BlendDistance", Range( 2 , 20)) = 2
		[Toggle]_L_Enable_Blending("L_Enable_Blending", Float) = 0
		[Toggle]_R_Enable_Blending("R_Enable_Blending", Float) = 0
		_V_BlendDistance("V_BlendDistance", Range( 2 , 20)) = 2
		[Toggle]_T_Enable_Blending("T_Enable_Blending", Float) = 0
		[Toggle]_B_Enable_Blending("B_Enable_Blending", Float) = 0
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		

		Pass
		{
			Name "Unlit"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			uniform float _L_Enable_Blending;
			uniform float _H_BlendDistance;
			uniform float _R_Enable_Blending;
			uniform float _B_Enable_Blending;
			uniform float _V_BlendDistance;
			uniform float _T_Enable_Blending;
			uniform sampler2D _Texture0;
			uniform float2 _Tiling;
			uniform float2 _Offset;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				
				v.vertex.xyz +=  float3(0,0,0) ;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				fixed4 finalColor;
				float2 uv10 = i.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_13_0 = (uv10.x*1.0 + 0.0);
				float clampResult25 = clamp( ( temp_output_13_0 * _H_BlendDistance ) , 0.0 , 1.0 );
				float clampResult26 = clamp( ( _H_BlendDistance * (1.0 + (temp_output_13_0 - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) ) , 0.0 , 1.0 );
				float temp_output_48_0 = (uv10.y*1.0 + 0.0);
				float clampResult53 = clamp( ( temp_output_48_0 * _V_BlendDistance ) , 0.0 , 1.0 );
				float clampResult54 = clamp( ( _V_BlendDistance * (1.0 + (temp_output_48_0 - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) ) , 0.0 , 1.0 );
				float2 uv5 = i.ase_texcoord.xy * _Tiling + _Offset;
				
				
				finalColor = ( ( lerp(1.0,clampResult25,_L_Enable_Blending) * lerp(1.0,clampResult26,_R_Enable_Blending) * lerp(1.0,clampResult53,_B_Enable_Blending) * lerp(1.0,clampResult54,_T_Enable_Blending) ) * tex2D( _Texture0, uv5 ) );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16100
2063;62;1666;849;1675.004;854.4185;1.487201;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-2132.785,-1287.717;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;13;-1613.069,-1698.675;Float;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;48;-1618.062,-995.3693;Float;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;21;-1335.131,-1349.174;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1309.999,-1600.565;Float;False;Property;_H_BlendDistance;H_BlendDistance;3;0;Create;True;0;0;False;0;2;0;2;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;52;-1247.07,-668.1599;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-1295.117,-887.8289;Float;False;Property;_V_BlendDistance;V_BlendDistance;6;0;Create;True;0;0;False;0;2;2;2;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-1028.248,-1414.608;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-1016.811,-1686.526;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-929.9575,-1050.016;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-931.0472,-783.9099;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;7;-800,-14.5;Float;False;Property;_Offset;Offset;2;0;Create;False;0;0;False;0;0,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;6;-800,-135.5;Float;False;Property;_Tiling;Tiling;1;0;Create;False;0;0;False;0;0.6,1;0.6,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ClampOpNode;26;-651.0433,-1407.305;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;54;-653.5757,-799.5912;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-550.7983,-1609.956;Float;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;53;-653.5754,-1037.811;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;25;-705.2958,-1793.418;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;46;-333.9839,-1834.112;Float;False;Property;_L_Enable_Blending;L_Enable_Blending;4;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;2;-788.614,-416.5396;Float;True;Property;_Texture0;Texture 0;0;0;Create;False;0;0;False;0;a17f849c6f1ea024c813a3ecdb0e6e1a;a17f849c6f1ea024c813a3ecdb0e6e1a;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-607.4356,-131.5;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;47;-321.9839,-1432.112;Float;False;Property;_R_Enable_Blending;R_Enable_Blending;5;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;55;-313.502,-1082.842;Float;False;Property;_B_Enable_Blending;B_Enable_Blending;8;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;56;-319.266,-844.2713;Float;False;Property;_T_Enable_Blending;T_Enable_Blending;7;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-385.8318,-316.7573;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;33.67378,-1058.431;Float;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-2.86719,-543.5343;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;194.9107,-540.8435;Float;False;True;2;Float;ASEMaterialInspector;0;1;Blending;0770190933193b94aaa3065e307002fa;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;13;0;10;1
WireConnection;48;0;10;2
WireConnection;21;0;13;0
WireConnection;52;0;48;0
WireConnection;22;0;19;0
WireConnection;22;1;21;0
WireConnection;16;0;13;0
WireConnection;16;1;19;0
WireConnection;49;0;48;0
WireConnection;49;1;51;0
WireConnection;50;0;51;0
WireConnection;50;1;52;0
WireConnection;26;0;22;0
WireConnection;54;0;50;0
WireConnection;53;0;49;0
WireConnection;25;0;16;0
WireConnection;46;0;43;0
WireConnection;46;1;25;0
WireConnection;5;0;6;0
WireConnection;5;1;7;0
WireConnection;47;0;43;0
WireConnection;47;1;26;0
WireConnection;55;0;43;0
WireConnection;55;1;53;0
WireConnection;56;0;43;0
WireConnection;56;1;54;0
WireConnection;1;0;2;0
WireConnection;1;1;5;0
WireConnection;27;0;46;0
WireConnection;27;1;47;0
WireConnection;27;2;55;0
WireConnection;27;3;56;0
WireConnection;9;0;27;0
WireConnection;9;1;1;0
WireConnection;0;0;9;0
ASEEND*/
//CHKSM=6636D2686979EBC5F81B7FF50EED9727488CFE40
// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "HitEffect"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_CircleColor("CircleColor", Color) = (1,0,0,0)
		_Transition("Transition", Range( -1 , 1)) = 1
		_TransitionSize("Transition Size", Range( 0 , 1)) = 0.05
		[Toggle]_ToggleSwitch0("Toggle Switch0", Float) = 0

	}

	SubShader
	{
		LOD 0

		
		
		ZTest Always
		Cull Off
		ZWrite Off

		
		Pass
		{ 
			CGPROGRAM 

			

			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata_img_custom
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				
			};

			struct v2f_img_custom
			{
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
				half2 stereoUV : TEXCOORD2;
		#if UNITY_UV_STARTS_AT_TOP
				half4 uv2 : TEXCOORD1;
				half4 stereoUV2 : TEXCOORD3;
		#endif
				float4 ase_texcoord4 : TEXCOORD4;
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform float4 _CircleColor;
			uniform float _ToggleSwitch0;
			uniform float _Transition;
			uniform float _TransitionSize;


			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord4 = screenPos;
				
				o.pos = UnityObjectToClipPos( v.vertex );
				o.uv = float4( v.texcoord.xy, 1, 1 );

				#if UNITY_UV_STARTS_AT_TOP
					o.uv2 = float4( v.texcoord.xy, 1, 1 );
					o.stereoUV2 = UnityStereoScreenSpaceUVAdjust ( o.uv2, _MainTex_ST );

					if ( _MainTex_TexelSize.y < 0.0 )
						o.uv.y = 1.0 - o.uv.y;
				#endif
				o.stereoUV = UnityStereoScreenSpaceUVAdjust ( o.uv, _MainTex_ST );
				return o;
			}

			half4 frag ( v2f_img_custom i ) : SV_Target
			{
				#ifdef UNITY_UV_STARTS_AT_TOP
					half2 uv = i.uv2;
					half2 stereoUV = i.stereoUV2;
				#else
					half2 uv = i.uv;
					half2 stereoUV = i.stereoUV;
				#endif	
				
				half4 finalColor;

				// ase common template code
				float4 screenPos = i.ase_texcoord4;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 originalUVs3 = (ase_screenPosNorm).xy;
				float2 Refraction5 = originalUVs3;
				float4 originalColor17 = tex2D( _MainTex, Refraction5 );
				float temp_output_36_0 = ( ( 1.0 - _Transition ) * 1.4142 );
				float2 temp_output_20_0 = (float2( 0,0 ) + (originalUVs3 - float2( 0,0 )) * (float2( 1,1 ) - float2( 0,0 )) / (float2( 1,1 ) - float2( 0,0 )));
				float smoothstepResult44 = smoothstep( ( temp_output_36_0 - _TransitionSize ) , ( temp_output_36_0 + _TransitionSize ) , length( temp_output_20_0 ));
				float temp_output_23_0 = ( _ScreenParams.x / _ScreenParams.y );
				float temp_output_40_0 = ( temp_output_23_0 * temp_output_36_0 );
				float2 break21 = temp_output_20_0;
				float4 appendResult26 = (float4(( break21.x * temp_output_23_0 ) , break21.y , 0.0 , 0.0));
				float smoothstepResult43 = smoothstep( ( temp_output_40_0 - _TransitionSize ) , ( temp_output_40_0 + _TransitionSize ) , length( appendResult26 ));
				float4 lerpResult31 = lerp( originalColor17 , _CircleColor , saturate( (( _ToggleSwitch0 )?( smoothstepResult43 ):( smoothstepResult44 )) ));
				float4 newColor32 = lerpResult31;
				

				finalColor = newColor32;

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18934
274.4;73.6;882;463.8;1783.423;-518.3492;1.474677;True;False
Node;AmplifyShaderEditor.ScreenPosInputsNode;1;-1873.771,-38.59792;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;2;-1660.176,-31.85274;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;3;-1662.424,67.07558;Inherit;False;originalUVs;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-1250.667,445.4289;Inherit;False;3;originalUVs;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;20;-992.2375,440.7302;Inherit;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;0,0;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1274.209,926.9498;Inherit;False;Property;_Transition;Transition;1;0;Create;True;0;0;0;False;0;False;1;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenParams;22;-1186.059,726.1766;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;34;-987.5877,935.1726;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-995.8104,1018.575;Inherit;False;Constant;_Root2;Root2;2;0;Create;True;0;0;0;False;0;False;1.4142;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;23;-1007.508,748.4955;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;21;-1005.159,633.3773;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-839.5292,612.2328;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;4;-1898.881,241.1299;Inherit;False;3;originalUVs;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-811.3857,952.7927;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-1261.287,1168.933;Inherit;False;Property;_TransitionSize;Transition Size;2;0;Create;True;0;0;0;False;0;False;0.05;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-556.4802,933.9979;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;5;-1608.574,239.1816;Inherit;False;Refraction;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;26;-691.5197,635.7265;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LengthOpNode;27;-544.6851,639.2505;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;39;-703.3154,1221.794;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;28;-551.733,447.7782;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-698.6166,1117.247;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;42;-399.0734,1234.715;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;6;-1374.77,-19.95193;Inherit;False;5;Refraction;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;15;-1157.382,-96.13632;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-395.5494,1113.723;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;43;-258.1118,892.8842;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;44;-258.1109,740.1761;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;-1019.256,-40.88748;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;45;-43.14518,814.1807;Inherit;False;Property;_ToggleSwitch0;Toggle Switch0;3;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;17;-682.1232,-5.647151;Inherit;False;originalColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;-1255.365,320.9131;Inherit;False;17;originalColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;46;181.4929,822.9527;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;-333.2426,390.2192;Inherit;False;Property;_CircleColor;CircleColor;0;0;Create;True;0;0;0;False;0;False;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;31;-107.7046,319.7385;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;32;77.89504,323.2625;Inherit;False;newColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;-214.3663,-7.814797;Inherit;False;32;newColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;2;HitEffect;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;True;7;False;-1;False;True;0;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;2;0;1;0
WireConnection;3;0;2;0
WireConnection;20;0;19;0
WireConnection;34;0;33;0
WireConnection;23;0;22;1
WireConnection;23;1;22;2
WireConnection;21;0;20;0
WireConnection;25;0;21;0
WireConnection;25;1;23;0
WireConnection;36;0;34;0
WireConnection;36;1;35;0
WireConnection;40;0;23;0
WireConnection;40;1;36;0
WireConnection;5;0;4;0
WireConnection;26;0;25;0
WireConnection;26;1;21;1
WireConnection;27;0;26;0
WireConnection;39;0;36;0
WireConnection;39;1;37;0
WireConnection;28;0;20;0
WireConnection;38;0;36;0
WireConnection;38;1;37;0
WireConnection;42;0;40;0
WireConnection;42;1;37;0
WireConnection;41;0;40;0
WireConnection;41;1;37;0
WireConnection;43;0;27;0
WireConnection;43;1;42;0
WireConnection;43;2;41;0
WireConnection;44;0;28;0
WireConnection;44;1;39;0
WireConnection;44;2;38;0
WireConnection;16;0;15;0
WireConnection;16;1;6;0
WireConnection;45;0;44;0
WireConnection;45;1;43;0
WireConnection;17;0;16;0
WireConnection;46;0;45;0
WireConnection;31;0;18;0
WireConnection;31;1;29;0
WireConnection;31;2;46;0
WireConnection;32;0;31;0
WireConnection;0;0;47;0
ASEEND*/
//CHKSM=91AA45D68A9E353FDB464691412F4F1C449231C6
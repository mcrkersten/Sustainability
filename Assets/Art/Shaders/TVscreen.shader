// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2225,x:32754,y:32755,varname:node_2225,prsc:2|diff-9306-OUT,emission-6750-RGB;n:type:ShaderForge.SFN_Tex2d,id:7056,x:32281,y:32773,ptovrint:False,ptlb:StaticTexture,ptin:_StaticTexture,varname:node_7056,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c645771e82678784a9e2f1a5005e6aaf,ntxv:2,isnm:False|UVIN-9255-UVOUT;n:type:ShaderForge.SFN_Panner,id:9255,x:32094,y:32628,varname:node_9255,prsc:2,spu:0,spv:1|UVIN-9241-UVOUT,DIST-266-OUT;n:type:ShaderForge.SFN_TexCoord,id:9241,x:31841,y:32616,varname:node_9241,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Divide,id:266,x:32094,y:32860,varname:node_266,prsc:2|A-4928-T,B-1702-OUT;n:type:ShaderForge.SFN_Time,id:4928,x:31739,y:32793,varname:node_4928,prsc:2;n:type:ShaderForge.SFN_RemapRange,id:1702,x:31885,y:32938,varname:node_1702,prsc:2,frmn:0,frmx:1,tomn:1,tomx:10|IN-2871-OUT;n:type:ShaderForge.SFN_Slider,id:2871,x:31507,y:32963,ptovrint:False,ptlb:node_2871,ptin:_node_2871,varname:node_2871,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Tex2d,id:6750,x:32316,y:32500,ptovrint:False,ptlb:CameraTexture,ptin:_CameraTexture,varname:node_6750,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:73d151b527d371845a635449b582020a,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Blend,id:9306,x:32484,y:32733,varname:node_9306,prsc:2,blmd:10,clmp:True|SRC-6750-RGB,DST-7056-RGB;proporder:7056-2871-6750;pass:END;sub:END;*/

Shader "Custom/TVscreen" {
    Properties {
        _StaticTexture ("StaticTexture", 2D) = "black" {}
        _node_2871 ("node_2871", Range(0, 1)) = 0
        _CameraTexture ("CameraTexture", 2D) = "white" {}
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}
		ColorMask[_ColorMask]
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _StaticTexture; uniform float4 _StaticTexture_ST;
            uniform float _node_2871;
            uniform sampler2D _CameraTexture; uniform float4 _CameraTexture_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _CameraTexture_var = tex2D(_CameraTexture,TRANSFORM_TEX(i.uv0, _CameraTexture));
                float4 node_4928 = _Time;
                float2 node_9255 = (i.uv0+(node_4928.g/(_node_2871*9.0+1.0))*float2(0,1));
                float4 _StaticTexture_var = tex2D(_StaticTexture,TRANSFORM_TEX(node_9255, _StaticTexture));
                float3 diffuseColor = saturate(( _StaticTexture_var.rgb > 0.5 ? (1.0-(1.0-2.0*(_StaticTexture_var.rgb-0.5))*(1.0-_CameraTexture_var.rgb)) : (2.0*_StaticTexture_var.rgb*_CameraTexture_var.rgb) ));
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = _CameraTexture_var.rgb;
/// Final Color:
                float3 finalColor = diffuse + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _StaticTexture; uniform float4 _StaticTexture_ST;
            uniform float _node_2871;
            uniform sampler2D _CameraTexture; uniform float4 _CameraTexture_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _CameraTexture_var = tex2D(_CameraTexture,TRANSFORM_TEX(i.uv0, _CameraTexture));
                float4 node_4928 = _Time;
                float2 node_9255 = (i.uv0+(node_4928.g/(_node_2871*9.0+1.0))*float2(0,1));
                float4 _StaticTexture_var = tex2D(_StaticTexture,TRANSFORM_TEX(node_9255, _StaticTexture));
                float3 diffuseColor = saturate(( _StaticTexture_var.rgb > 0.5 ? (1.0-(1.0-2.0*(_StaticTexture_var.rgb-0.5))*(1.0-_CameraTexture_var.rgb)) : (2.0*_StaticTexture_var.rgb*_CameraTexture_var.rgb) ));
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

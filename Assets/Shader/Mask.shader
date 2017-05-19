// Shader created with Shader Forge v1.36 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.36;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32887,y:32660,varname:node_3138,prsc:2|emission-4499-OUT,alpha-7826-OUT;n:type:ShaderForge.SFN_Tex2d,id:5679,x:31902,y:33170,ptovrint:False,ptlb:Pattern,ptin:_Pattern,varname:node_5679,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:03f90b9462ee44a4e8391fbf48999277,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:7303,x:31902,y:32619,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:_node_5679_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0ee88fdf76548f04ca9e39348a5508a7,ntxv:0,isnm:False;n:type:ShaderForge.SFN_If,id:4499,x:32319,y:32800,varname:node_4499,prsc:2|A-7303-RGB,B-8636-RGB,GT-5273-RGB,EQ-5679-RGB,LT-5273-RGB;n:type:ShaderForge.SFN_Color,id:8636,x:31902,y:33004,ptovrint:False,ptlb:White,ptin:_White,varname:node_8636,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Color,id:5273,x:31902,y:32821,ptovrint:False,ptlb:Black,ptin:_Black,varname:node_5273,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:0;n:type:ShaderForge.SFN_If,id:7826,x:32412,y:33007,varname:node_7826,prsc:2|A-7303-R,B-8636-R,GT-278-OUT,EQ-7855-OUT,LT-278-OUT;n:type:ShaderForge.SFN_Vector1,id:278,x:32178,y:33173,varname:node_278,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:7855,x:32178,y:33260,varname:node_7855,prsc:2,v1:1;proporder:5679-7303-8636-5273;pass:END;sub:END;*/

Shader "Shader Forge/Mask" {
    Properties {
        _Pattern ("Pattern", 2D) = "black" {}
        _Mask ("Mask", 2D) = "white" {}
        _White ("White", Color) = (1,1,1,1)
        _Black ("Black", Color) = (0,0,0,0)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Pattern; uniform float4 _Pattern_ST;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float4 _White;
            uniform float4 _Black;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float node_4499_if_leA = step(_Mask_var.rgb,_White.rgb);
                float node_4499_if_leB = step(_White.rgb,_Mask_var.rgb);
                float4 _Pattern_var = tex2D(_Pattern,TRANSFORM_TEX(i.uv0, _Pattern));
                float3 emissive = lerp((node_4499_if_leA*_Black.rgb)+(node_4499_if_leB*_Black.rgb),_Pattern_var.rgb,node_4499_if_leA*node_4499_if_leB);
                float3 finalColor = emissive;
                float node_7826_if_leA = step(_Mask_var.r,_White.r);
                float node_7826_if_leB = step(_White.r,_Mask_var.r);
                float node_278 = 0.0;
                return fixed4(finalColor,lerp((node_7826_if_leA*node_278)+(node_7826_if_leB*node_278),1.0,node_7826_if_leA*node_7826_if_leB));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

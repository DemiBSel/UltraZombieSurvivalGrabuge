Shader "Custom/playerShader" {
     Properties 
     {
         _Color ("Main Color", Color) = (1,1,1,1)
         _MainTex ("Texture", 2D) = "white" {}
     }
     


		 SubShader {
			  Tags {
			  "Queue"="Overlay+1" }

			 Pass
             {
                 //ZWrite Off
                 ZTest Greater
                 Lighting Off
                 Color [_Color]
				 SetTexture [_MainTex] {combine texture}
             }
		} 
     
     FallBack "Diffuse"
}

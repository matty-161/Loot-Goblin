shader_type canvas_item;

uniform bool enabled = false;
uniform float progress = 0.0;
uniform vec3 shock_color : source_color = vec3(1.0, 0.0, 0.0);

void fragment() {
	if (enabled){
		float exponent = mod(progress, 3);
		vec3 normal_color = texture(TEXTURE, UV).rgb;
		COLOR.rgb = normal_color + shock_color * exp(-3.0*exponent);
	}
	
}

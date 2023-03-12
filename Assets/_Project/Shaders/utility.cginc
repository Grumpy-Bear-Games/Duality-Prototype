//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED



void sdEllipse_float( float2 p, float2 ab, out float distance )
{
    // symmetry
    p = abs( p );
    
    // initial value
    float2 q = ab*(p-ab);
    float2 cs = normalize( (q.x<q.y) ? float2(0.01,1) : float2(1,0.01) );
    
    // find root with Newton solver
    for( int i=0; i<5; i++ )
    {
        float2 u = ab*float2( cs.x,cs.y);
        float2 v = ab*float2(-cs.y,cs.x);
        float a = dot(p-u,v);
        float c = dot(p-u,u) + dot(v,v);
        float b = sqrt(c*c-a*a);
        cs = float2( cs.x*b-cs.y*a, cs.y*b+cs.x*a )/c;
    }
    
    // compute final point and distance
    float d = length(p-ab*cs);
    
    // return signed distance
    distance = (dot(p/ab,p/ab)>1.0) ? d : -d;
}

void sdEllipse_half( half2 p, half2 ab, out half distance )
{
    // symmetry
    p = abs( p );
    
    // initial value
    half2 q = ab*(p-ab);
    half2 cs = normalize( (q.x<q.y) ? half2(0.01,1) : half2(1,0.01) );
    
    // find root with Newton solver
    for( int i=0; i<5; i++ )
    {
        half2 u = ab*half2( cs.x,cs.y);
        half2 v = ab*half2(-cs.y,cs.x);
        float a = dot(p-u,v);
        float c = dot(p-u,u) + dot(v,v);
        float b = sqrt(c*c-a*a);
        cs = half2( cs.x*b-cs.y*a, cs.y*b+cs.x*a )/c;
    }
    
    // compute final point and distance
    float d = length(p-ab*cs);
    
    // return signed distance
    distance = (dot(p/ab,p/ab)>1.0) ? d : -d;
}

#endif //MYHLSLINCLUDE_INCLUDED

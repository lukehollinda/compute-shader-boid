# compute-shader-boid

Unity project simulating the flocking behaviour of birds. The original concept is from [Craid Reynolds' paper](https://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.103.7187)
published in 1987 for the ACM SIGGRAPH conference. 

In my first year of university I wrote a 2D version of this in Processing. This project is both nostalgic and a way for me to dip my toes in compute shaders and High-level shader language (HLSL).

Each little rocket calculates it's own position, velocity, and spherecasts to avoid obstacles. The separation, cohesion, and alignment forces are calculated on the GPU using a computer shader, allowing us to control upwards of 3000 rockets. 


https://user-images.githubusercontent.com/47929615/127237578-29da4f42-4ccc-426c-9715-d6d9a52c6ee8.mp4



https://user-images.githubusercontent.com/47929615/127237581-021a2dc9-5110-4462-91ae-94ab3c0f0a1e.mp4


# Geometry 2D
 
A 2D geometry library supporting the following basic 2D primitive shapes:


**Circles, Triangles, Infinite Lines, Line Segments, Quads, Polygons**

All shapes are structs so there is little heap allocation except for Polygon type which uses a list. 

Polymorphism is supported via interfaces. And intersections for each shape is also supported. Each shape also supports a variety of helpful functions.

<b>Note:</b> Naming convention does not follow standard C# convention. This is because I wrote a lot of this originally in HLSL and converted it to C#.

## Demo

https://github.com/DaveTheCelt/Geometry2D/assets/130871588/d1dd5dd3-74df-4492-a787-0c0c6d1c00e0

## Functions supported

<b>All</b> geometry objects have the following functions:
<ul>
        <li>Tangent -> <i>Get the tangent vector nearest to the point</i></li>
        <li>Normal -> <i>Get the normal vector nearest to the point</i></li>
        <li>Translate -> <i>Move the position of the object</i></li>
        <li>SetPosition -> <i>Set the position of the object</i></li>
        <li>Scale -> <i>(Not including infinite lines) </i></li>
        <li>Rotate -> <i>Rotate the object</i></li>
        <li>CalculateBounds -> <i>Get the AABB bounds of the shape</i></li>
        <li>OverlapsEdge -> <i>Check if point overlaps an edge</i></li>
        <li>SnapTo -> <i>Snap a point to the nearest edge</i></li>
        <li>CalculatePerimeter -> <i>Calculate the perimeter of the line/shape</i></li>
</ul>
<b>Lines</b> have the following additional functions:
<ul>
        <li>Lerp ->  <i>Interpolate along the line</i></li>
        <li>InverseLerp ->  <i>Get the parameterised value from a point</i></li>
        <li>IsBetween ->  <i>Is a point between the two end points of the line</i></li>
        <li>Split ->  <i>Split the line into 2 lines</i></li>
        <li>Slice ->  <i>Slice the line into multiple lines</i></li>
        <li>Truncate ->  <i>Shorten the line</i></li>
        <li>IsLeft ->  <i>Is a point left of the line</i></li>
</ul>
<b>2DShapes</b> have the following additional functions:
<ul>
        <li>CalculateArea -> <i>Calculate area of the shape</i></li>
        <li>Overlaps -> <i>Check if a point overlaps the shape</i></li>
</ul>



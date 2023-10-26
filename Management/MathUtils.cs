using UnityEngine;
using System.Collections;

public static class MathUtils
{

	public static readonly Vector3 INVALID = new Vector3(float.NaN, float.NaN, float.NaN);

    /**
     * For a line from p1 to p2, find the intersections with a circle, if any.
     * see http://stackoverflow.com/questions/1073336/circle-line-segment-collision-detection-algorithm
     */
    public static int LineIntersectsCircle(Vector2 p1, Vector2 p2, Vector2 circleCenter, float radius, out Vector2 intersection1, out Vector2 intersection2)
	{
		Vector2 d = p2 - p1;
		Vector2 f = p1 - circleCenter;

		float a = Vector2.Dot(d, d);
		float b = 2 * Vector2.Dot(f, d);
		float c = Vector2.Dot(f, f) - radius * radius;

		float discriminant = b * b - 4 * a * c;

		if (Mathf.Approximately(discriminant, 0))
		{
			// tangential: one intersection
			float t = -b / (2 * a);
			intersection1 = intersection2 = p1 + t * d;
			return 1;
		}

		if (discriminant < 0)
		{
            // no intersection
            intersection1 = intersection2 = INVALID;
            return 0;
		}
		else
		{
			// ray didn't totally miss sphere,
			// so there is a solution to
			// the equation.

			discriminant = Mathf.Sqrt(discriminant);

			// either solution may be on or off the ray so need to test both
			// t1 is always the smaller value, because BOTH discriminant and
			// a are nonnegative.
			float t1 = (-b - discriminant) / (2 * a);
			float t2 = (-b + discriminant) / (2 * a);

			// 3x HIT cases:
			//          -o->             --|-->  |            |  --|->
			// Impale(t1 hit,t2 hit), Poke(t1 hit,t2>1), ExitWound(t1<0, t2 hit), 

			// 3x MISS cases:
			//       ->  o                     o ->              | -> |
			// FallShort (t1>1,t2>1), Past (t1<0,t2<0), CompletelyInside(t1<0, t2>1)

			int intersections = 0;

			if (t1 >= 0 && t1 <= 1)
			{
				// t1 is an intersection, and if it hits,
				// it's closer than t2 would be
				// Impale, Poke
				intersection1 = p1 + t1 * d;
				++intersections;
			}
            else
            {
                intersection1 = INVALID;
            }

            if (t2 >= 0 && t2 <= 1)
			{
				// ExitWound
				intersection2 = p1 + t2 * d;
				++intersections;
			}
            else
            {
                intersection2 = INVALID;
            }
            
            // when solutions == 0: no intersection: FallShort, Past, CompletelyInside
            return intersections;
		}
	}

    public static bool isValid(this Vector2 p)
    {
        return !float.IsNaN(p.x) && !float.IsNaN(p.y);
    }

    public static bool isValid(this Vector3 p)
    {
        return !float.IsNaN(p.x) && !float.IsNaN(p.y) && !float.IsNaN(p.z);
    }

}

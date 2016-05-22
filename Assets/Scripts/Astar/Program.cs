using System;
using System.Collections.Generic;
 
using System.Linq;
using System.Text;
 

namespace  AStar 
{
    /// <summary>
    /// A simple console routine to show examples of the A* implementation in use
    /// </summary>
   static class  Program
    {
        private static bool[,] map;
        public static SearchParameters searchParameters;



        /// <summary>
        /// Outputs three examples of path finding to the Console.
        /// </summary>
        /// <remarks>The examples have copied from the unit tests!</remarks>
        /*   public void Run()
           {
               // Start with a clear map (don't add any obstacles)
               InitializeMap();
               PathFinder pathFinder = new PathFinder(searchParameters);
               List<Point> path = pathFinder.FindPath();
               ShowRoute("The algorithm should find a direct path without obstacles:", path);
               Console.WriteLine();

               // Now add an obstacle
               InitializeMap();
               AddWallWithGap();
               pathFinder = new PathFinder(searchParameters);
               path = pathFinder.FindPath();
               ShowRoute("The algorithm should find a route around the obstacle:", path);
               Console.WriteLine();

               // Finally, create a barrier between the start and end points
               InitializeMap();
               AddWallWithoutGap();
               pathFinder = new PathFinder(searchParameters);
               path = pathFinder.FindPath();
               ShowRoute("The algorithm should not be able to find a route around the barrier:", path);
               Console.WriteLine();

               Console.WriteLine("Press any key to exit...");

           }*/

        /// <summary>
        /// Displays the map and path as a simple grid to the console
        /// </summary>
        /// <param name="title">A descriptive title</param>
        /// <param name="path">The points that comprise the path</param>
            /*public static void ShowRoute(string title, IEnumerable<Point> path)
             {
                 Console.WriteLine("{0}\r\n", title);
                 for (int y =  map.GetLength(1) - 1; y >= 0 ; y--) // Invert the Y-axis so that coordinate 0,0 is shown in the bottom-left
                 {
                     for (int x = 0; x <  map.GetLength(0); x++)
                     {
                         if ( searchParameters.StartLocation.Equals(new Point(x, y)))
                             // Show the start position
                             Console.Write('S');
                         else if ( searchParameters.EndLocation.Equals(new Point(x, y)))
                             // Show the end position
                             Console.Write('F');
                         else if ( map[x, y] == false)
                             // Show any barriers
                             Console.Write('X');
                         else if (path.Where(p => p.X == x && p.Y == y).Any())
                             // Show the path in between
                             Console.Write('*');
                         else
                             // Show nodes that aren't part of the path
                             Console.Write('.');
                     }

                     Console.WriteLine();
                 }
             }
             */
        public static List<Point> FindPath(Point startLocation, Point finLocation, bool diagonal = false  )
        {
            searchParameters = new SearchParameters(startLocation, finLocation, map);
            //InitializeMap();
            PathFinder pathFinder = new PathFinder(searchParameters);
            List<Point> path = pathFinder.FindPath();
            if (diagonal) PathFinder.diagonal = true;
            else PathFinder.diagonal = false;
            return path;
        }
        /// <summary>
        /// Creates a clear map with a start and end point and sets up the search parameters
        /// </summary>
        /// 

        public static void InitializeMap()
        {
         

            map = new bool[9, 9];
            for (int y = 0; y <9; y++)
                for (int x = 0; x < 9; x++)
                    map[x, y] = true;
/*
            var startLocation = StartLocation;
            var endLocation = EndLocation;
             searchParameters = new SearchParameters(startLocation, endLocation, map);*/
        }

       
        public static void AddWall(int x, int y)
        {
           
            // No path

             map[x, y] = false;
        }
       public static void  RemoveWall(int x, int y)
        {
            
            // No path

             map[x, y] = true;
        }

    }
}

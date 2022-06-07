using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Galaga.Game
{
    // magic grid :)
    // note: it could be not so magic if i was allowed to use 3rd party tweeners
    public class Grid : MonoBehaviour
    {
        public int Width;
        public int Height;
        public float Amplitude;

        private Vector3[,] _grid;
        public List<string> _config;


        public void Configure(ConfigLevel configLevel)
        {
            Assert.IsNotNull(configLevel);
            _config = configLevel.MonsterGrid;

            // apply config
            Width = _config.Max(str => str.Length);
            Height = _config.Count;
            GenerateGrid();
        }

        void Update()
        {
            const float ResquadCycle = 20f;
            var val = BounceEaseOut((Time.realtimeSinceStartup % ResquadCycle) / ResquadCycle) * Amplitude;
            transform.localScale = new Vector3(0.85f + val * 0.5f, 1.1f + val * .045f, 1);
        }

        void GenerateGrid()
        {
            _grid = new Vector3[Width, Height];
            for (int x = 0; x < Width; ++x)
                for (int y = 0; y < Height; ++y)
                {
                    _grid[x, y] = new Vector3(x - Width * 0.5f + 0.5f, y, 0);
                }
        }

        public string GetRawConfig(int raw)
        {
            return _config[raw];
        }

        public Vector3 Get(int x, int y)
        {
            Assert.IsTrue(x >= 0 && x < Width && y >= 0 && y < Height, x + " " + y);
            var vec = _grid[x, 4 - y];
            vec.x *= transform.localScale.x;
            vec.y *= transform.localScale.y;
            return transform.position + vec;
        }

        static float BounceEaseOut(float p)
        {
            if (p < 4 / 11.0f)
                return (121 * p * p) / 16.0f;
            if (p < 8 / 11.0f)
                return (363 / 40.0f * p * p) - (99 / 10.0f * p) + 17 / 5.0f;
            if (p < 9 / 10.0f)
                return (4356 / 361.0f * p * p) - (35442 / 1805.0f * p) + 16061 / 1805.0f;
            return (54 / 5.0f * p * p) - (513 / 25.0f * p) + 268 / 25.0f;
        }

        void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.red;
            for (int x = 0; x < Width; ++x)
                for (int y = 0; y < Height; ++y)
                {
                    Gizmos.DrawSphere(Get(x, y), 0.1f);
                }
        }
    }
}
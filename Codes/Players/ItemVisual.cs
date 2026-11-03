using UnityEngine;

namespace _01_Scripts.Players
{
    public static class ItemVisual
    {
        private static Material _material;
        public static Material Material
        {
            get
            {
                if (_material == null)
                {
                    Shader shader = Resources.Load<Shader>("ItemEffect");
                    if (shader == null) shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
                    if (shader == null) shader = Shader.Find("Sprites/Default");
                    _material = new Material(shader);
                }
                return _material;
            }
        }

        public static GameObject CreateOrb(string name, Vector3 position, Color color, float size)
        {
            GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            orb.name = name;
            orb.transform.position = position;
            orb.transform.localScale = Vector3.one * size;
            Collider collider = orb.GetComponent<Collider>();
            collider.enabled = false;
            Object.Destroy(collider);
            Renderer renderer = orb.GetComponent<Renderer>();
            renderer.sharedMaterial = Material;
            var properties = new MaterialPropertyBlock();
            properties.SetColor("_BaseColor", color);
            properties.SetColor("_Color", color);
            renderer.SetPropertyBlock(properties);
            return orb;
        }

        public static void Burst(Vector3 position, Color color, float radius)
        {
            GameObject effect = new GameObject("Item Impact");
            effect.transform.position = position;
            ParticleSystem particles = effect.AddComponent<ParticleSystem>();
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var main = particles.main;
            main.loop = false;
            main.startLifetime = 0.35f;
            main.startSpeed = radius * 2;
            main.startSize = 0.12f;
            main.startColor = color;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            var emission = particles.emission;
            emission.rateOverTime = 0;
            var shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.1f;
            particles.GetComponent<ParticleSystemRenderer>().sharedMaterial = Material;
            particles.Play();
            particles.Emit(24);
            Object.Destroy(effect, 1f);
        }

        public static void RicochetLink(Vector3 start, Vector3 end)
        {
            GameObject effect = new GameObject("Ricochet Lightning");
            LineRenderer line = effect.AddComponent<LineRenderer>();
            line.sharedMaterial = Material;
            line.useWorldSpace = true;
            line.positionCount = 5;
            Vector3 side = Vector3.Cross((end - start).normalized, Vector3.up) * 0.15f;
            for (int i = 0; i < 5; i++)
                line.SetPosition(i, Vector3.Lerp(start, end, i / 4f) + (i == 0 || i == 4 ? Vector3.zero : side * (i % 2 == 0 ? -1 : 1)));
            line.startWidth = 0.07f;
            line.endWidth = 0.025f;
            line.startColor = line.endColor = PlayerItems.ItemColor(ItemType.Ricochet);
            Object.Destroy(effect, 0.12f);
        }

        public static void FrostBurst(Vector3 position)
        {
            Burst(position, PlayerItems.ItemColor(ItemType.Frost), 0.7f);
            for (int i = 0; i < 4; i++)
            {
                float angle = i * Mathf.PI * 0.5f;
                GameObject shard = CreateOrb("Ice Shard", position + new Vector3(Mathf.Cos(angle), 0.2f, Mathf.Sin(angle)) * 0.25f,
                    new Color(0.65f, 0.9f, 1f), 0.1f);
                shard.transform.localScale = new Vector3(0.07f, 0.35f, 0.07f);
                shard.transform.rotation = Quaternion.Euler(25, i * 90, 30);
                Object.Destroy(shard, 0.25f);
            }
        }
    }
}

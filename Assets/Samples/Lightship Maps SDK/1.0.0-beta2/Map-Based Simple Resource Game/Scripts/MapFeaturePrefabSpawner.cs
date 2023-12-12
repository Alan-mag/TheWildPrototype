// Copyright 2022 Niantic, Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using Niantic.Lightship.Maps.Features;
using Niantic.Lightship.Maps.Logging;
using Niantic.Lightship.Maps.Unity.Builders.BaseTypes;
using Niantic.Lightship.Maps.Unity.Core;
using Niantic.Lightship.Maps.Unity.Utilities;
using UnityEngine;

namespace Niantic.Lightship.Maps.Samples.GameSample
{
    using PooledObjectDictionary = Dictionary<GameObject, PooledObject<GameObject>>;

    /// <summary>
    /// Custom maptile feature builder that spawns a prefab for features
    /// found on the map.  Spawned prefabs are attached directly to active
    /// map tiles.  Only supports area map features, so not paths or roads.
    /// </summary>
    [CreateAssetMenu(fileName = "MapFeaturePrefabSpawner", menuName = "Lightship Maps/Builders/Game Prefab Spawner")]
    public class MapFeaturePrefabSpawner : ObjectBuilder
    {
        [SerializeField]
        private GameObject _prefab;

        [SerializeField]
        private MapGameState.ResourceType _resourceType;

        private static ChannelLogger Log { get; } = new ChannelLogger(nameof(MapFeaturePrefabSpawner));

        private ILightshipMap _lightshipMap;
        private ObjectPool<GameObject> _objectPool;
        private readonly PooledObjectDictionary _pooledObjects = new PooledObjectDictionary();

        /// <inheritdoc />
        public override void Initialize(ILightshipMap lightshipMap)
        {
            base.Initialize(lightshipMap);
            _lightshipMap = lightshipMap;
            _objectPool = new ObjectPool<GameObject>(_prefab);
        }

        /// <inheritdoc />
        public override void Build(IMapTile mapTile, GameObject parent)
        {
            if (!MapGameState.Instance.IsResourceProductionEnabled(_resourceType))
            {
                // Early-out if this type of resource production isn't enabled
                return;
            }

            foreach (var featureKind in Features)
            {
                var features = mapTile.GetTileData(Layer, featureKind);

                foreach (var feature in features)
                {
                    if (feature is IAreaFeature areaFeature)
                    {
                        // Find the center of this area feature
                        var centroid = MeshBuilderUtils.CalculateCentroid(areaFeature.Points);

                        // Get or create a prefab instance from the object pool
                        var pooledObject = _objectPool.GetOrCreate();
                        var featureInstance = pooledObject.Value;
                        _pooledObjects.Add(featureInstance, pooledObject);

                        // Enable and un-hide this object (if it was pooled)
                        featureInstance.hideFlags &= ~HideFlags.HideInHierarchy;
                        featureInstance.SetActive(true);

                        // Hook this up to the parent and set its position and scale
                        var tileScale = 1.0f / (_lightshipMap.MapScale * mapTile.Size);
                        featureInstance.transform.SetParent(parent.transform, false);
                        featureInstance.transform.localPosition = centroid;
                        featureInstance.transform.localScale = (float)tileScale * Vector3.one;
                    }
                }
            }
        }

        /// <inheritdoc />
        public override void Release(GameObject parent)
        {
            while (parent.transform.childCount > 0)
            {
                var child = parent.transform.GetChild(0);
                var childGameObject = child.gameObject;

                if (!_pooledObjects.Remove(childGameObject, out var pooledObject))
                {
                    // If we can't find this child in our list of pooled objects,
                    // just destroy it rather than releasing it back to the pool
                    Log.Warning("Couldn't find a pooled object to release!");
                    childGameObject.transform.SetParent(null, false);
                    Destroy(childGameObject);
                    continue;
                }

                // Detach this child object from its parent
                childGameObject.transform.SetParent(null, false);

                // Disable and hide this object in the hierarchy
                childGameObject.hideFlags |= HideFlags.HideInHierarchy;
                childGameObject.SetActive(false);

                // Return the GameObject to the pool
                pooledObject.Dispose();
            }
        }
    }
}

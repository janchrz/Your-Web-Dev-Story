using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gyvr.Mythril2D
{
    public class InstancePool : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject m_instancePrefab = null;

        [Header("Settings")]
        [SerializeField] private bool m_disableInstancesOnSceneUnloaded = false;
        [SerializeField] private string m_instancePrefix = "Instance_";
        [SerializeField] protected int m_poolSize = 3;
        [SerializeField] private bool m_autoDisableInstances = true;
        [SerializeField] private bool m_attachInstancesToParent = false;

        protected GameObject[] m_instances = null;

        public GameObject[] instances => m_instances;

        private void Awake()
        {
            m_instances = new GameObject[m_poolSize];

            for (int i = 0; i < m_poolSize; ++i)
            {
                GameObject instance = Instantiate(m_instancePrefab, Vector3.zero, Quaternion.identity, transform);

                // We do this instead of passing null as the parent transform to the Instantiate() function to ensure that the instance stays in the same scene as the InstancePool.
                // See https://forum.unity.com/threads/instantiating-objects-into-a-specific-scene.459108/
                if (!m_attachInstancesToParent)
                {
                    instance.transform.parent = null;
                }

                instance.name = m_instancePrefix + i;

                if (m_autoDisableInstances)
                {
                    instance.SetActive(false);
                }

                m_instances[i] = instance;
            }

            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;

            foreach (GameObject instance in m_instances)
            {
                Destroy(instance);
            }
        }

        private void OnSceneUnloaded(Scene scene)
        {
            if (m_disableInstancesOnSceneUnloaded)
            {
                DisableAll();
            }
        }

        public GameObject GetAvailableInstance()
        {
            GameObject instance = Array.Find(m_instances, (element) => !element.activeInHierarchy);

            if (!instance)
            {
                Debug.LogWarning("Could not find available instance in pool, consider expanding the pool size");
            }

            return instance;
        }

        public void DisableAll()
        {
            for (int i = 0; i < m_poolSize; ++i)
            {
                m_instances[i].SetActive(false);
            }
        }
    }
}

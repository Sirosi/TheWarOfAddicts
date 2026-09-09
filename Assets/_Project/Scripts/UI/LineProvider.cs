using TheWarOfAddicts.GameFlow.StageSelect;
using UnityEngine;

namespace TheWarOfAddicts.UI
{
    public class LineProvider: MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private StageNode[] nodes;
        
        
        [ContextMenu("Set Lines")]
        private void Apply()
        {
            if (nodes == null || nodes.Length <= 0) return;
            
            if (!lineRenderer)
            {
                lineRenderer = GetComponent<LineRenderer>();
            }

            lineRenderer.positionCount = nodes.Length;
            int n = 0;
            foreach (StageNode node in nodes)
            {
                lineRenderer.SetPosition(n++, node.transform.position);
            }
        }
    }
}
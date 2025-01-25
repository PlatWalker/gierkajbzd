using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace jbzd
{
    [ExecuteAlways]
    public class AutoLoadPipelineAsset : MonoBehaviour
    {
        public UniversalRenderPipelineAsset pipelineAsset;
        
        private void OnEnable()
        {
            if (pipelineAsset)
            {
                GraphicsSettings.defaultRenderPipeline = pipelineAsset;
                GraphicsSettings.defaultRenderPipeline = pipelineAsset;
                QualitySettings.renderPipeline = pipelineAsset;
            }
        }
    }
}

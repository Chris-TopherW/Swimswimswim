//
//  Plugin_BitCrusher.cpp
//  AudioPluginDemo
//
//  Created by Chris Wratt on 21/08/16.
//
//

#include "AudioPluginUtil.h"

namespace BitSmasher
{
    enum Param
    {
        P_CRUSH,
        P_MIX,
        P_NUM
    };
    
    //Stores data for access within plugin
    struct EffectData
    {
        struct Data
        {
            float p[P_NUM];
            float increment;
            float currentBand;
        };
        union
        {
            Data data;
            unsigned char pad[(sizeof(Data) + 15) & ~15]; // This entire structure must be a multiple of 16 bytes (and and instance 16 byte aligned) for PS3 SPU DMA requirements
        };
    };
    
#if !UNITY_SPU
    
    //registers parameters with unity- creates sliders etc
    int InternalRegisterEffectDefinition(UnityAudioEffectDefinition& definition)
    {
        int numparams = P_NUM;
        definition.paramdefs = new UnityAudioParameterDefinition[numparams];
        RegisterParameter(definition, "Mix amount", "", 0.0f, 1.0f, 0.5f, 1.0f, 1.0f, P_MIX, "Ratio between input and ring-modulated signals");
        RegisterParameter(definition, "quantisation bands", "bands", 4.0, 6000.0f, 20.0f, 2.0f, 2000.0f, P_CRUSH, "");
        return numparams;
    }
    
    //instantiates effect and data, ie constructor
    UNITY_AUDIODSP_RESULT UNITY_AUDIODSP_CALLBACK CreateCallback(UnityAudioEffectState* state)
    {
        EffectData* effectdata = new EffectData;
        memset(effectdata, 0, sizeof(EffectData));
        state->effectdata = effectdata;
        effectdata->data.increment = 0.1f;
        effectdata->data.currentBand = 0.0f;
        InitParametersFromDefinitions(InternalRegisterEffectDefinition, effectdata->data.p);
        return UNITY_AUDIODSP_OK;
    }
    
    //Cleans up memory when plugin is deleted/ closed. ie destructor
    UNITY_AUDIODSP_RESULT UNITY_AUDIODSP_CALLBACK ReleaseCallback(UnityAudioEffectState* state)
    {
        EffectData::Data* data = &state->GetEffectData<EffectData>()->data;
        delete data;
        return UNITY_AUDIODSP_OK;
    }
    
    //Is this for passing data to C#??
    UNITY_AUDIODSP_RESULT UNITY_AUDIODSP_CALLBACK SetFloatParameterCallback(UnityAudioEffectState* state, int index, float value)
    {
        EffectData::Data* data = &state->GetEffectData<EffectData>()->data;
        if (index >= P_NUM)
            return UNITY_AUDIODSP_ERR_UNSUPPORTED;
        data->p[index] = value;
        return UNITY_AUDIODSP_OK;
    }
    
    UNITY_AUDIODSP_RESULT UNITY_AUDIODSP_CALLBACK GetFloatParameterCallback(UnityAudioEffectState* state, int index, float* value, char *valuestr)
    {
        EffectData::Data* data = &state->GetEffectData<EffectData>()->data;
        if (index >= P_NUM)
            return UNITY_AUDIODSP_ERR_UNSUPPORTED;
        if (value != NULL)
            *value = data->p[index];
        if (valuestr != NULL)
            valuestr[0] = 0;
        return UNITY_AUDIODSP_OK;
    }
    
    int UNITY_AUDIODSP_CALLBACK GetFloatBufferCallback(UnityAudioEffectState* state, const char* name, float* buffer, int numsamples)
    {
        return UNITY_AUDIODSP_OK;
    }
    
#endif
    
#if !UNITY_PS3 || UNITY_SPU
    
#if UNITY_SPU
    EffectData  g_EffectData __attribute__((aligned(16)));
    extern "C"
#endif
    //all audio dsp is done here
    UNITY_AUDIODSP_RESULT UNITY_AUDIODSP_CALLBACK ProcessCallback(UnityAudioEffectState* state, float* inbuffer, float* outbuffer, unsigned int length, int inchannels, int outchannels)
    {
        EffectData::Data* data = &state->GetEffectData<EffectData>()->data;
        
#if UNITY_SPU
        UNITY_PS3_CELLDMA_GET(&g_EffectData, state->effectdata, sizeof(g_EffectData));
        data = &g_EffectData.data;
#endif
        
        for (unsigned int n = 0; n < length; n++)
        {
            for (int i = 0; i < outchannels; i++)
            {
                data->increment = 1.0f / data->p[P_CRUSH];
                data->currentBand = 0;
                while(data->currentBand <= 1.0f)
                {
                    data->currentBand += data->increment;
                    if(inbuffer[n * outchannels + i] < data->currentBand && inbuffer[n * outchannels + i] > data->currentBand - data->increment){
                        outbuffer[n * outchannels + i] = data->currentBand;
                    }
                   outbuffer[n * outchannels + i] = outbuffer[n * outchannels + i] * (1.0f - data->p[P_MIX]) + inbuffer[n * outchannels + i] * data->p[P_MIX];
                }
            }
        }
        
#if UNITY_SPU
        UNITY_PS3_CELLDMA_PUT(&g_EffectData, state->effectdata, sizeof(g_EffectData));
#endif
        
        return UNITY_AUDIODSP_OK;
    }
    
#endif
}

using UnityEngine;
using Unity.Profiling;
using System.Text;

public class MemoryProfiler : MonoBehaviour
{
    string _statsText;
    ProfilerRecorder _totalReservedMemoryRecorder;
    ProfilerRecorder _totalUsedMemoryRecorder;
    ProfilerRecorder _audioUsedMemoryRecorder;
    ProfilerRecorder _textureMemoryRecorder;
    ProfilerRecorder _meshMemoryRecorder;
    void OnEnable()
    {
        _totalReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
        _totalUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
        _audioUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Audio Used Memory");
        _textureMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Texture Memory");
        _meshMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Mesh Memory");
    }
    void OnDisable()
    {
        _totalReservedMemoryRecorder.Dispose();
        _totalUsedMemoryRecorder.Dispose();
        _audioUsedMemoryRecorder.Dispose();
        _textureMemoryRecorder.Dispose();
        _meshMemoryRecorder.Dispose();
    }
    void Update()
    {
        var sb = new StringBuilder(500);
        if (_totalReservedMemoryRecorder.Valid)
            sb.AppendLine($"Total Reserved Memory: {_totalReservedMemoryRecorder.LastValue}");
        if (_totalUsedMemoryRecorder.Valid)
            sb.AppendLine($"Total Used Memory: {_totalUsedMemoryRecorder.LastValue}");
        if (_audioUsedMemoryRecorder.Valid)
            sb.AppendLine($"Audio Used Memory: {_audioUsedMemoryRecorder.LastValue}");
        if (_textureMemoryRecorder.Valid)
            sb.AppendLine($"Texture Used Memory: {_textureMemoryRecorder.LastValue}");
        if (_meshMemoryRecorder.Valid)
            sb.AppendLine($"Mesh Used Memory: {_meshMemoryRecorder.LastValue}");
        _statsText = sb.ToString();
    }
    void OnGUI()
    {
        GUI.TextArea(new Rect(10, 40, 250, 70), _statsText);
    }
}
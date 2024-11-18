using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class KochLine : KochGenerator
{
    public MazecraftGameManager instance;
     LineRenderer _lineRenderer;
     [Range(0,1)]
     public float _lerpAmount;
     Vector3[] _lerpPosition;
     public float _generateMultiplier;
     int currentIteration = 0;
     int iterationsLimit = 3;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = true;
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = _position.Length;
        _lineRenderer.SetPositions(_position);

        InvokeRepeating("BuildFractal", 10, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if(_generationCount != 0){
            for(int i = 0; i < _position.Length; i++){
                _lerpPosition[i] = Vector3.Lerp(_position[i], _targetPosition[i], _lerpAmount);
            }
            _lineRenderer.SetPositions(_lerpPosition);
        }
    }

    void BuildFractal(){
        if(currentIteration <= iterationsLimit){
            bool random = true || false;

            if(random == true){
                Outward();
            }
            else if(random == false){
                Inward();
            }
            currentIteration++;
        }
        else if(currentIteration > iterationsLimit){
            CancelInvoke();
            this.gameObject.GetComponent<RotateSGeometry>().enabled = true;
        }
    }

    private void Outward(){
        KochGenerate(_targetPosition, true, _generateMultiplier);
        _lerpPosition = new Vector3[_position.Length];
        _lineRenderer.positionCount = _position.Length;
        _lineRenderer.SetPositions(_position);
        _lerpAmount = 0;
    }

    private void Inward(){
        KochGenerate(_targetPosition, false, _generateMultiplier);
        _lerpPosition = new Vector3[_position.Length];
        _lineRenderer.positionCount = _position.Length;
        _lineRenderer.SetPositions(_position);
        _lerpAmount = 0;
    }
}

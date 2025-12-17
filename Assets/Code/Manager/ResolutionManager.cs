using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 해상도 관리자. 
// 플레이하는 디바이스의 해상도와 제작할때 기준이 된 해상도가 서로 다를때,
// 레터박스와 세터박스의 기법을 활용해서 제작한 비율이 유지가 되도록. 
public class ResolutionManager :SingletonDestroy<ResolutionManager>
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasScaler canvasScalier;

    private Vector2 fixedAspectRatio = new Vector2(16,9);

    protected override void DoAwake()
    {
        base.DoAwake(); // 부모클래스의 내용을 한번 실행 후에 본인의 doAwake를 호출.

        // 초기설정 
        ApplySetting();
    }

    private void ApplySetting()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }

        if (canvas == null)
        {
            canvas = FindObjectOfType<Canvas>();
        }

        if (canvasScalier == null)
            canvasScalier = FindObjectOfType<CanvasScaler>();


        if (mainCam != null)
        {
            //해상도 고정
            SetCameraAspectRatio();
        }

        if (canvas != null && canvasScalier != null)
        {
            ConfigureCanvas();
        }
    }

    private void SetCameraAspectRatio()
    {
        // 카메라의 사각형 (rect) 정보를 가져옴
        Rect rt = mainCam.rect;

        // 현재 화면의 종횡비 계산
        float screenAspect = (float)Screen.width / Screen.height;

        // 고정된 종횡비 계산
        float targetAspect = fixedAspectRatio.x / fixedAspectRatio.y;

        // 현재 화면 종횡비가 목표 종횡비보다 크거나 같으면
        if (screenAspect >= targetAspect)
        {
            // 화면의 너비를 조정하여 목표 종횡비를 맞춤
            float width = targetAspect / screenAspect;
            rt.width = width;
            rt.x = (1f - width) / 2f; // 좌우 여백 조정
            rt.height = 1f;
            rt.y = 0f; // 상하 여백 없음
        }
        else
        {
            // 화면의 높이를 조정하여 목표 종횡비를 맞춤
            float height = screenAspect / targetAspect;
            rt.width = 1f;
            rt.x = 0f; // 좌우 여백 없음
            rt.height = height;
            rt.y = (1f - height) / 2f; // 상하 여백 조정
        }

        // 카메라의 사각형 (rect) 설정 적용
        mainCam.rect = rt;
    }

    private void ConfigureCanvas()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = mainCam;
        canvas.planeDistance = 1f;

        canvasScalier.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScalier.referenceResolution = new Vector2(1920, 1080);
        canvasScalier.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScalier.matchWidthOrHeight = 0.5f;
    }
}
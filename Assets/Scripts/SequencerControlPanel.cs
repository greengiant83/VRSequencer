using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerControlPanel : MonoBehaviour
{
    public static SequencerControlPanel Instance { get; private set; }

    [SerializeField] private InputGrid InputGrid;
    [SerializeField] private EnvelopeEditor EnvelopeEditor;
    [SerializeField] private Transform PanelSizeReference;

    private Sequencer source;
    
    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (source != null && source.IsMoving) movePanelToSequencer();
    }


    public void Show(Sequencer Sequencer)
    {
        gameObject.SetActive(true);
        source = Sequencer;
        InputGrid.SetSource(source);
        EnvelopeEditor.SetSource(source);
        movePanelToSequencer();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void movePanelToSequencer()
    {
        if (source == null) return;

        var sourceSize = source.GetSize();
        transform.rotation = source.transform.rotation;
        transform.position = source.transform.position + source.transform.forward * -(sourceSize.z / 2) + Vector3.down * (sourceSize.y / 2 + PanelSizeReference.localScale.y / 2);

        EnvelopeEditor.transform.rotation = source.transform.rotation;
        EnvelopeEditor.transform.position = source.transform.position + source.transform.right * sourceSize.x / 2 + source.transform.up * -sourceSize.y / 2;
        EnvelopeEditor.SetDepth(sourceSize.z);
    }
}

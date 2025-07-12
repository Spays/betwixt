using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ResurcesInterface
{
    public int resursIndex { get; set; }
    public int resursValue { get; set; }
    void OnTriggerEnter2D(Collider2D other);
    void SelfKill();
}

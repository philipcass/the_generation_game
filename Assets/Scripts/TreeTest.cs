using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using PH.DataTree;
using System.Collections.Generic;

public class TreeTest : WMG_Graph_Manager {
	public GameObject CardPrefab;
	public GameObject LinkPrefab;
	public int radius;
	public DTreeNode<CardModel> root;
	
	
	// Use this for initialization
	void Start () {
		root = new DTreeNode<CardModel>(new CardModel());
		RefreshTree ();
		
	}
	
	void CalcWeights (DTreeNode<CardModel> card)
	{
		Traverse<CardModel> (card, delegate (DTreeNode<CardModel> c) {
			c.Value.Weight = 0;
			c.Value.used_weight = 0;
			if (!c.HasChildren) {
				c.Value.Weight = 1;
			}
			else {
				foreach (var item in c.Nodes) {
					c.Value.Weight += item.Value.Weight;
				}
			}
		});
	}
	
	public void RefreshTree ()
	{
		CalcWeights (root);
		Dictionary<int, int> weight_counts = new Dictionary<int, int> ();
		foreach (var item in root.BreadthFirstNodeEnumerator) {
			if (item.Value.go == null) {
				item.Value.go = this.CreateNode (this.CardPrefab, this.gameObject);
				item.Value.go.GetComponent<CardControls>().Node = item;
				item.Value.go.GetComponent<CardControls>().MyTree = this;
			}

			if(item.Parent == null){
				item.Value.centre = 0;
			}else{
				item.Value.centre = (item.Parent.Value.centre) + item.Parent.Value.used_weight;
				item.Parent.Value.used_weight += item.Value.Weight;
			}


			item.Value.go.GetComponent<WMG_Node> ().Reposition (item.Value.centre * this.radius, -item.Depth * this.radius);
		}
		foreach (var item in root.BreadthFirstNodeEnumerator) {
			if (item.HasChildren) {
				foreach (var child in item.Nodes) {
					this.CreateLink (item.Value.go.GetComponent<WMG_Node> (), child.Value.go.GetComponent<WMG_Node> ());
				}
			}
		}
	}
	
	public static void Traverse<T>(DTreeNode<T> node, Action<DTreeNode<T>> visitor)
	{
		foreach (DTreeNode<T> kid in node.Nodes)
			Traverse(kid, visitor);
		visitor(node);
	}        
	
	public void CreateLink(WMG_Node fromNode, WMG_Node toNode) {
		WMG_Link aLink = GetLink(fromNode, toNode);
		if (aLink == null) {
			CreateLink(fromNode, toNode.gameObject, this.LinkPrefab, this.gameObject);
		}
	}
	
}
/*
 * PROPRIETARY INFORMATION.  This software is proprietary to
 * Side Effects Software Inc., and is not to be reproduced,
 * transmitted, or disclosed in any way without written permission.
 *
 * Produced by:
 *      Side Effects Software Inc
 *		123 Front Street West, Suite 1401
 *		Toronto, Ontario
 *		Canada   M5J 2M2
 *		416-504-9876
 *
 * COMMENTS:
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoudiniObjectControl : HoudiniControl 
{
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Public Properties
	
	// Please keep these in the same order and grouping as their initializations in HAPI_Control.reset().

	public int		prObjectId {		get { return myObjectId; }			set { myObjectId = value; } }
	public string	prObjectName {		get { return myObjectName; }		set { myObjectName = value; } }
	public bool		prObjectVisible {	get { return myObjectVisible; }		set { myObjectVisible = value; } }

	public List< GameObject > prGeos {	get { return myGeos; }				set { myGeos = value; } }

	public HoudiniObjectControl() 
	{
		reset();
	}

	~HoudiniObjectControl()
	{

	}

	public override void reset()
	{
		base.reset();

		// Please keep these in the same order and grouping as their declarations at the top.
		
		myObjectId		= -1;
		myObjectName	= "object_name";
		myObjectVisible	= false;

		myGeos = new List< GameObject >( 0 );
	}

	public void init( HoudiniObjectControl object_control )
	{
		init( (HoudiniControl) object_control );

		prObjectId		= object_control.prObjectId;
		prObjectName	= object_control.prObjectName;
		prObjectVisible = object_control.prObjectVisible;
	}

	public void init( 
		int asset_id, int node_id, HoudiniAsset asset, int object_id, string object_name, bool object_visible )
	{
		prAssetId		= asset_id;
		prNodeId		= node_id;
		prAsset			= asset;
		prObjectNodeId	= node_id;
		prObjectId		= object_id;
		prObjectName	= object_name;
		prObjectVisible = object_visible;
	}

	public override string getRelativePath()
	{
		string name = prObjectName;

		// This is necessary so we return consistent paths regardless of the 
		// name of the asset node which can be named differently for different
		// instantiations of the asset (ie geo1, geo2, etc.)
		if ( prAsset.prNodeId == prObjectNodeId )
		{
			name = prAsset.prAssetOpName;
		}

		return base.getRelativePath() + "/" + name;
	}

	public override string getAbsolutePath()
	{
		string asset_name = "";

		// If this object is NOT the asset node, we want to include the
		// asset node name in the path.
		if ( prAsset.prNodeId != prObjectNodeId )
		{
			asset_name = prAsset.prAssetName + "/";
		}

		return base.getAbsolutePath() + "/" + asset_name + prObjectName;
	}

	public bool refresh( bool reload_asset, HAPI_ObjectInfo object_info )
	{
		bool needs_recook = false;

		if ( reload_asset )
		{
			for ( int i = 0; i < myGeos.Count; ++i )
				HoudiniAssetUtility.destroyGameObject( myGeos[ i ] );
			myGeos.Clear();
		}

		if ( reload_asset || object_info.haveGeosChanged )
		{
			// Add new geos as needed.
			while ( myGeos.Count < object_info.geoCount )
				myGeos.Add( createGeo( myGeos.Count ) );

			// Remove stale geos.
			while ( myGeos.Count > object_info.geoCount )
			{
				HoudiniAssetUtility.destroyGameObject( myGeos[ object_info.geoCount ] );
				myGeos.RemoveAt( object_info.geoCount );
			}

			// Refresh all geos.
			for ( int i = 0; i < myGeos.Count; ++i )
				needs_recook |= myGeos[ i ].GetComponent< HoudiniGeoControl >().refresh( reload_asset );
		}

		return needs_recook;
	}

	public void beginBakeAnimation()
	{
		myCurveCollection = new HoudiniCurvesCollection();
	}

	public void bakeAnimation( float curr_time, GameObject parent_object, HAPI_Transform hapi_transform )
	{
		try
		{
			Matrix4x4 parent_xform_inverse = Matrix4x4.identity;

			if ( parent_object != null )
				parent_xform_inverse = parent_object.transform.localToWorldMatrix.inverse;

			Vector3 pos = new Vector3();
			
			// Apply object transforms.
			//
			// Axis and Rotation conversions:
			// Note that Houdini's X axis points in the opposite direction that Unity's does.  Also, Houdini's 
			// rotation is right handed, whereas Unity is left handed.  To account for this, we need to invert
			// the x coordinate of the translation, and do the same for the rotations (except for the x rotation,
			// which doesn't need to be flipped because the change in handedness AND direction of the left x axis
			// causes a double negative - yeah, I know).
			
			pos[ 0 ] = -hapi_transform.position[ 0 ];
			pos[ 1 ] =  hapi_transform.position[ 1 ];
			pos[ 2 ] =  hapi_transform.position[ 2 ];
			
			Quaternion quat = new Quaternion( 	hapi_transform.rotationQuaternion[ 0 ],
												hapi_transform.rotationQuaternion[ 1 ],
												hapi_transform.rotationQuaternion[ 2 ],
												hapi_transform.rotationQuaternion[ 3 ] );
			
			Vector3 euler = quat.eulerAngles;
			euler.y = -euler.y;
			euler.z = -euler.z;
								
			quat = Quaternion.Euler( euler );
			
			Vector3 scale = new Vector3 ( hapi_transform.scale[ 0 ],
										  hapi_transform.scale[ 1 ],
										  hapi_transform.scale[ 2 ] );
			
			if( parent_object != null )
			{
				Matrix4x4 world_mat = Matrix4x4.identity;
				world_mat.SetTRS( pos, quat, scale );
				Matrix4x4 local_mat = parent_xform_inverse  * world_mat;
				
				quat = HoudiniAssetUtility.getQuaternion( local_mat );
				scale = HoudiniAssetUtility.getScale( local_mat );
				pos = HoudiniAssetUtility.getPosition( local_mat );
			}

			HoudiniCurvesCollection curves = myCurveCollection;
			
			addKeyToCurve( curr_time, pos[ 0 ], curves.tx );
			addKeyToCurve( curr_time, pos[ 1 ], curves.ty );
			addKeyToCurve( curr_time, pos[ 2 ], curves.tz );
			addKeyToCurve( curr_time, quat.x, curves.qx );
			addKeyToCurve( curr_time, quat.y, curves.qy );
			addKeyToCurve( curr_time, quat.z, curves.qz );
			addKeyToCurve( curr_time, quat.w, curves.qw );
			addKeyToCurve( curr_time, scale.x, curves.sx );
			addKeyToCurve( curr_time, scale.y, curves.sy );
			addKeyToCurve( curr_time, scale.z, curves.sz );
		}
		catch ( HoudiniError error )
		{
			Debug.LogWarning( error.ToString() );
			return;
		}
	}

	public bool endBakeAnimation()
	{
		try
		{
			HoudiniCurvesCollection curves = myCurveCollection;
			AnimationClip clip = curves.assignCurvesToClip();

			if ( clip != null )
			{
				Animation anim_component = gameObject.GetComponent< Animation >();
				if ( anim_component == null )
				{
					gameObject.AddComponent< Animation >();
					anim_component = gameObject.GetComponent< Animation >();
				}

				anim_component.clip = clip;
				return true;
			}
			
			return false;
		}
		catch ( HoudiniError error )
		{
			Debug.LogWarning( error.ToString() );
			return false;
		}
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Private Methods

	private GameObject createGeo( int geo_id )
	{
		GameObject child = new GameObject( "uninitialized_geo" );
		child.transform.parent = gameObject.transform;
		child.isStatic = gameObject.isStatic;

		// Need to reset position here because the assignment above will massage the child's
		// position in order to be in the same place it was in the global namespace.
		child.transform.localPosition	= new Vector3();
		child.transform.localRotation	= new Quaternion();
		child.transform.localScale		= new Vector3( 1.0f, 1.0f, 1.0f );

		HoudiniGeoControl control = child.AddComponent< HoudiniGeoControl >();
		control.init( this );
		control.prGeoId = geo_id;
		control.prObjectControl = this;

		return child;
	}

	private void addKeyToCurve( float time, float val, AnimationCurve curve )
	{
		Keyframe curr_key = new Keyframe( time, val, 0, 0 );
		curve.AddKey( curr_key );
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Serialized Private Data

	[SerializeField] private int		myObjectId;
	[SerializeField] private string		myObjectName;
	[SerializeField] private bool		myObjectVisible;

	[SerializeField] private List< GameObject > myGeos;
	
	private HoudiniCurvesCollection myCurveCollection = null;
}

	var ListeServeurs = [];
	var SiteActuel = 0;
	

// enregistre les informations d'un site
function ClickBouton(){
	// Lancement de l'application 
	 if (!window.sessionStorage){
        return;
	 }
	//alert("Lancement de l'application"+ SiteActuel);
	sessionStorage.SiteActuel= SiteActuel;
	setTimeout(function(){document.location.href = "homiweb.html"; },200);
}  


// stocke tous les informations de site dans un tableau
function ReadSites(){
	  if (!window.localStorage){
        return;
    }
	// Supprime tous les icones deja exitantes
	v_div_parent = document.getElementById("ListeServeurs");
	while (v_div_parent.firstChild) {
  		v_div_parent.removeChild(v_div_parent.firstChild);
}
	
	var Cpt = localStorage.length; 
	
	if  (Cpt)  {
	
	// REdessine toutes les icones
		for (i=0;i<Cpt;i++) {
			key = localStorage.key(i);
			InfosServeur = JSON.parse(localStorage.getItem(key));
			ListeServeurs.push(InfosServeur);
			document.getElementById('ListeServeurs').innerHTML += '<div><img src="images/icones/home_128.png" onclick="AfficheSite(' + i +')" /><span>'+ InfosServeur.SERVER + '</span></div>';
			document.getElementById('ListeServeurs').style.display = 'inline' ;
		}
	}
	else 
	{
		document.getElementById('ListeServeurs').style.display = 'none' ;
	}
}	

// Affiche les infos du site selectionn�
function AfficheSite(IndexSite){
	// alert("Passage par la fonction alert avec le parametre : "+IndexSite)
	document.getElementById('ServerName').value = ListeServeurs[IndexSite].SERVER;
	document.getElementById('ServerID').value = ListeServeurs[IndexSite].IDSERVER;
	document.getElementById('ServerIP').value = ListeServeurs[IndexSite].AdresseIP;
	document.getElementById('ServerPort').value = ListeServeurs[IndexSite].Port;
	SiteActuel = IndexSite;
}

// Affiche les infos du site selectionn�
function SupprimeSite(){
	if (ListeServeurs.length) {
		// alert("Passage par la fonction SupprimeSite avec le parametre : "+SiteActuel + " - " +ListeServeurs[SiteActuel].NomSite)  ;	
		localStorage.removeItem(ListeServeurs[SiteActuel].NomSite);
		ListeServeurs.splice(SiteActuel,1);	
		ReadSites();
	}
	
}

// enregistre les informations d'un site
function AjouterSite(){
	var InfosServeur = new Object();

	if (!window.localStorage){
        return;
    }
    var Cpt = localStorage.length;	

	
	var ServerName = document.getElementById('ServerName').value;
	var ServerID = document.getElementById('ServerID').value;
	var ServerIP = document.getElementById('ServerIP').value;
	var ServerPort = document.getElementById('ServerPort').value;
	if (!ServerName=="" && !ServerID=="" && !ServerIP=="" && !ServerPort=="") {
		var NomSite = "Site"+"::"+ Cpt ;
		InfosServeur.NomSite    = NomSite;
		InfosServeur.SERVER    = ServerName;
		InfosServeur.IDSERVER  = ServerID;
		InfosServeur.AdresseIP = ServerIP;
		InfosServeur.Port      = ServerPort;
		localStorage.setItem(NomSite, JSON.stringify(InfosServeur));
		ReadSites(); 
	}
	else {
		alert('Veuillez saisir tous les champs ! ');
	}
}  
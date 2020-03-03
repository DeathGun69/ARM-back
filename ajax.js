$(document).ready(function(){
    let keys_all={  'group':'группы',
                    'competence' :'КОМПЕТЕНЦИИ',
                    'course':'курсы',
                    'name':'название',
                    'univer':'университет',
                    'hours':'количество часов',
                    'id_group':'группы',
                    'id_competence':'помпетенции',
                    'id_teacher':'учителя'    ,
                    'code':"код",
                    'theme':"темы",
                    'user':"преподаватели",
                    'surname':'фамилия',
                    'patronymic':'отчество',
                     'login':'логин',
                     'is_admin':'пароль'
            
                }

   
$('.singin').on('click',function(){ 
  Token();
  if ((token=='Invalid token')||(token == undefined)){   
    $('.er').empty()
    $('.er').append('проверьте логин и пароль') }else{
  authorization(token,keys_all)
    }
})
  

    
})
function Token(){
    let request=1


        let request_z='http://195.46.123.219:22555/?request={"method":"auth", "params":{"login":"'+$('#log').val()+'","password":"'+$('#pas').val()+'"}} '
    
        console.log(request_z)
        $.ajax({
            type:'post',
            url:request_z,
            async:false,
            cach: false,
            error:(function(){
                alert('ошибка')}),
            success:function(data){ 

                token=data.result
                
                return token;
            }
        }); 
   
}


function authorization(token,keys_all){
        
       console.log(token)
       z=$('#log').val()
       $('#s1').remove();
       $('#z').remove();
       $('head').append('<link id="z" rel="stylesheet" type="text/css" href="stylet.css" >')
       if(z=='admin'){
     alert(11)
        $('.n').remove()
        $( '#menu ul' ).append('<li class="menu" id="user"><a href="#">учителя</a></li>')
        $( '#menu ul' ).append('<li class="menu" id="competence"><a href="#">компетенции</a></li>')
        page(keys_all,token,'course')

       }else{page(keys_all,token,'course')}
        $('.menu').on('click',function(){ 
          page(keys_all,token,$(this).attr('id'))})
    
      }
        

function form(token,table,keys,keys_all){
    
    $( ".modal-title" ).empty() 
    $( ".modal-body" ).empty() 
    $( ".modal-footer" ).empty() 
    $( ".modal-title" ).append( 'Добавление ' ) 
    
    for (let i = 0; i < keys.length; i++) {                                            
        switch (keys[i]){
            case 'id':
            break;

            case 'surname':
            case 'patronymic':
            case 'login':
            case 'is_admin':
            case 'code':
            case 'name':
            case 'univer':
            case 'hours':modal1(keys[i],keys_all);
            break;
            case 'id_group':modal2(token,keys[i],keys_all,'group');
            break;
            case 'id_competence':modal2(token,keys[i],keys_all,'competence');
            break;
            case 'id_teacher':modal2(token,keys[i],keys_all,'user');
            break;
        }
    }
    form_add_button(token,table,keys);
}
function modal1(key,keys_all){
    $( ".modal-body" ).append( ' <p>'+keys_all[key]+'</p>' ) 
    $( ".modal-body" ).append( '<input id="'+key+'" type="text" placeholder="введите данные">' ) 
    $( ".modal-body" ).append( '    <br></br>    ' ) 
}
function modal2(token, key,keys_all,z){
    $( ".modal-body" ).append( '<p>'+keys_all[key]+'</p>' ) 
    $( ".modal-body" ).append( ' <div class="'+key+'"><div class="cl1"><select id="'+key+'" ></select></div></div>' ) 
    request_z='http://195.46.123.219:22555/?request={"method":"select","params":{"token":"'+token+'","table_name":"'+z+'"}}'
    $.ajax({
        type:'post',
        url:request_z,
        data: request,
        cach: false,
        error:(function(){alert('ошибка')}),
        success:function(data_g){  
            console.log(request_z)  
          console.log(data_g)    
          $( "#"+key).empty();
          for(i=0;i<data_g.result.length;i++){
            $( "#"+key ).append( ' <option value="'+data_g.result[i].id+'">'+data_g.result[i].name+'</option>' ) 
          }
        }
      });

}

function form_add_button(token,table,keys,keys_all){
    $( ".modal-footer" ).append( ' <div class="but">   <tr> <th><button class="buttonr" data-dismiss="modal">  отмена  </button></th> <th></th><button class="button b_add" value="'+table+'">добавить </button></th></tr></div>       ' ) 
   
}


function add (token,table,keys,keys_all){
    let request=''
    
   for(i=0;i<keys.length;i++){
       if (i==0){x=''}else{x=','}
       if(keys[i]=='name'||keys[i]=='univer'){
    request=request+x+'"'+keys[i]+'":"'+$('#'+keys[i]).val()+'"'}
    else if(keys[i]=='hours'||keys[i]=='id_group'||keys[i]=='id_competence'||keys[i]=='id_teacher'){request=request+x+'"'+keys[i]+'":'+$('#'+keys[i]).val()}
    else if(keys[i]=='id'){
        request=request+x+'"'+keys[i]+'":'+1 
    }
   
    
    
    
   
console.log(request)
   }
   let request_z='http://195.46.123.219:22555/?request={"method":"insert","params":{"token":"'+token+'","table_name":"'+table+'","rows":[{'+request+'}]}} '
    
    console.log('_____')
    console.log(request_z)
    $.ajax({
      type:'post',
      url:request_z,
      data: request,
      cach: false,
      error:(function(){alert('ошибка')}),
      success:function(data){ 
        console.log("eeee")
      console.log(data)
      page(keys_all,token,table)  
     
    
        ;
   
            
      }
    });
  }
function page(keys_all,token,table_name){

  $( ".container-fluid" ).empty();   
  $('#z').remove();
  $('head').append('<link id="z" rel="stylesheet" type="text/css" href="stylet.css" >')
  let request_z= 'http://195.46.123.219:22555/?request={"method":"select","params":{"token":"'+token+'","table_name":"'+table_name+'"}} '
  request=1
  $.ajax({
    async:false,
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(){console.log('ошибка')}),
    success:function(data){
      console.log(request_z)
      console.log(data)
      let keys=Object.keys(data.result[0])
      console.log(keys)
      $( ".page-wrap .container-fluid" ).append('<div class="inf col-12"> </div><div class="row"><table class="table table-striped table-hover"><div class="title"> <h1>'+keys_all[table_name]+'</h1></div> <thead class="tabt">   <tr ><th>назание</th><th><div class="cl"></div></th>  </tr>  </thead> <tbody class="tabb"></tbody></table> </div>   ')
      for(i=0;i<data.result.length;i++){
        $( ".tabb" ).append( '<tr id='+data.result[i].id+' class=>')
        $( '#'+data.result[i].id ).append( '<th>'+data.result[i].name+'</th>' )
        $( "#"+data.result[i].id ).append( '<th><button class="buttonr recours" value='+data.result[i].id+'>изменить</button><button class="del" value='+data.result[i].id+'>удалить</button></th>' ) 
      }    
      $( ".competenc_page .tabb" ).append( '</tr>') 
      $( ".page-wrap .container-fluid" ).append(' <a class="add" data-toggle="modal" data-target="#exampleModal" value="'+table_name+'" href="#" >ДОБАВИТЬ</a>')
      row_click(token,table_name,data)
      del_click(token,table_name)
      $('.add').on('click',function(){  
        form(token,table_name,keys,keys_all);          
        $('.b_add').on('click',function(){add(token,table_name,keys,keys_all);})
      })
    }
  });
    
}

function row_click(token,table,course){
  $('.tabb tr').on('click',function(){
    requst_inf($(this).attr('id'),table,token)


    inf_div=$(this).attr('id')
    alert(inf_div)

    //del(token,table,$(this).val());
  })
};

function requst_inf(id_el,id_name,token){
  console.log('id_el=='+id_el+"  id_name=="+id_name)

  let request_z= 'http://195.46.123.219:22555/?request={"method":"select","params":{"token":"'+token+'","table_name":"course_plan"}} '
  console.log(request_z)
  $.ajax({
    async:false,
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(){console.log('ошибка')}),
    success:function(data){
      console.log(data)
      
      /*
      console.log(request_z)
      console.log(data)
      let keys=Object.keys(data.result[0])
      console.log(keys)
      $( ".page-wrap .container-fluid" ).append('<div class="inf col-12"> </div><div class="row"><table class="table table-striped table-hover"><div class="title"> <h1>'+keys_all[table_name]+'</h1></div> <thead class="tabt">   <tr ><th>назание</th><th><div class="cl"></div></th>  </tr>  </thead> <tbody class="tabb"></tbody></table> </div>   ')
      for(i=0;i<data.result.length;i++){
        $( ".tabb" ).append( '<tr id='+data.result[i].id+' class=>')
        $( '#'+data.result[i].id ).append( '<th>'+data.result[i].name+'</th>' )
        $( "#"+data.result[i].id ).append( '<th><button class="buttonr recours" value='+data.result[i].id+'>изменить</button><button class="del" value='+data.result[i].id+'>удалить</button></th>' ) 
      }    
      $( ".competenc_page .tabb" ).append( '</tr>') 
      $( ".page-wrap .container-fluid" ).append(' <a class="add" data-toggle="modal" data-target="#exampleModal" value="'+table_name+'" href="#" >ДОБАВИТЬ</a>')
      row_click(token,table_name,data)
      del_click(token,table_name)
      $('.add').on('click',function(){  
        form(token,table_name,keys,keys_all);          
        $('.b_add').on('click',function(){add(token,table_name,keys,keys_all);})
      })
    */}
  });
}

function del_click(token,table){
    $('.del').on('click',function(){del(token,table,$(this).val());})
  };
  
  function del(token,table,zn){ 
    let request=1
    let request_z='http://195.46.123.219:22555/?request={"method":"delete","params":{"token":"'+token+'","table_name":"'+table+'","rows":['+zn+']}} '
    console.log('_____')
    console.log(request_z)
    $.ajax({
        
      type:'post',
      url:request_z,
      data: request,
      cach: false,
      error:(function(data){alert('ошибка')
      console.log(data);}),
      success:function(data){
        $( '#tr'+zn ).remove(); 
         }
      });
    }
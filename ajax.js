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
                    'is_admin':'админ',
                    'password':'пароль',
                    'id_section':'раздел',
                    'info':'описание'
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
  rc='http://195.46.123.219:22555/?request={"method":"doc","params":{"token":"'+token+'"}}'
     
       console.log(token)
       z=$('#log').val()
       $('#s1').remove();
       $('#z').remove();
       $('head').append('<link id="z" rel="stylesheet" type="text/css" href="stylet.css" >')
       if(z=='admin'){
     
        $('.n').remove()
        $( '#menu ul' ).append('<li class="menu" id="user"><a href="#">учителя</a></li>')
        $( '#menu ul' ).append('<li class="menu" id="competence"><a href="#">компетенции</a></li>')
        $( '#menu ul' ).append(" <li><a href='"+rc +"' download>Скачать файл</a></li>")
console.log(rc)
        page(keys_all,token,'course')

       }else{$( '#menu ul' ).append(" <li><a href='"+rc +"' download>Скачать файл</a></li>")
       page(keys_all,token,'course')}
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

           
            case 'id_group':modal2(token,keys[i],keys_all,'group');
            break;
            case 'id_section':modal2(token,keys[i],keys_all,'section');
            break;
            case 'id_competence':modal2(token,keys[i],keys_all,'competence');
            break;
            case 'id_teacher':modal2(token,keys[i],keys_all,'user');
            break;
            default:modal1(keys[i],keys_all,table);
            break;
        }
    }
    form_add_button(token,table,keys);
}
function modal1(key,keys_all,table){
  if (key=='name'&&table=='user'){ $( ".modal-body" ).append( ' <p>'+'имя'+'</p>' ) }else{$( ".modal-body" ).append( ' <p>'+keys_all[key]+'</p>' ) }
    if(key=='is_admin'){$( ".modal-body" ).append( '<input id="'+key+'" type="checkbox" name="админ">' ) }else if (key=='info'){ 
    
      $( ".modal-body" ).append( ' <textarea id="'+key+'" name="mesage" rows="4" cols="55" wrap="virtual"> введите данные</textarea>' )
    }else{
    $( ".modal-body" ).append( '<input id="'+key+'" type="text" placeholder="введите данные">' ) }
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
    $( ".modal-footer" ).append( ' <div class="but">   <tr> <th><button class="buttonr" data-dismiss="modal">  отмена  </button></th> <th></th><button class="button b_add" value="'+table+'" data-dismiss="modal">добавить </button></th></tr></div>       ' ) 
   
}


function add (token,table,keys,keys_all){
    let request=''
    
   for(i=0;i<keys.length;i++){
       if (i==0){x=''}else{x=','}
       if((keys[i]=='code'&&table!='group')||keys[i]=='info'||keys[i]=='patronymic'||keys[i]=='login'||keys[i]=='password'||keys[i]=='surname'||keys[i]=='name'||keys[i]=='univer'){
    request=request+x+'"'+keys[i]+'":"'+$('#'+keys[i]).val()+'"'}
    else if((keys[i]=='code'&&table=='group')||keys[i]=='id_section'||keys[i]=='hours'||keys[i]=='id_group'||keys[i]=='id_competence'||keys[i]=='id_teacher'){request=request+x+'"'+keys[i]+'":'+$('#'+keys[i]).val()}
    else if(keys[i]=='id'){
        request=request+x+'"'+keys[i]+'":'+1 
    }
    else if(keys[i]=='is_admin'){    request=request+x+'"'+keys[i]+'":'+$('#'+keys[i]).is(':checked')+''}
  
  
    
   
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
      if(table=="theme"){
       
      }
      else {page(keys_all,token,table)  }
     
    
        
   
            
      }
    });
  }
function page(keys_all,token,table_name,keys){

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
      console.log('вот  это место=====')
      console.log(data)
    
      let keys=Object.keys(data.result[0])
      console.log('вот  этот ключ=====')
      console.log(keys)
      $( ".page-wrap .container-fluid" ).append('<div class="inf col-12"> </div><div class="row"><table class="table table-striped table-hover"><div class="title"> <h1>'+keys_all[table_name]+'</h1></div> <thead class="tabt">   <tr ><th>назание</th><th><div class="cl"></div></th>  </tr>  </thead> <tbody class="tabb"></tbody></table> </div>   ')
      for(i=0;i<data.result.length;i++){
        $( ".tabb" ).append( '<tr id='+data.result[i].id+' class=>')
        $( '#'+data.result[i].id ).append( '<th>'+data.result[i].name+'</th>' )
        $( "#"+data.result[i].id ).append( '<th><button class="buttonr recours" value='+data.result[i].id+'>изменить</button><button class="del" value='+data.result[i].id+'>удалить</button></th>' ) 
      }    
      $( ".competenc_page .tabb" ).append( '</tr>') 
      $( ".page-wrap .container-fluid" ).append(' <a class="add" data-toggle="modal" data-target="#exampleModal" value="'+table_name+'" href="#" >ДОБАВИТЬ</a>')
      if(table_name=="course")row_click(token,table_name,keys,keys_all)
      del_click(token,table_name,keys_all)
      $('.add').on('click',function(){  
        form(token,table_name,keys,keys_all);          
        $('.b_add').on('click',function(){add(token,table_name,keys,keys_all);})
      })
    }
  });
    
}

function row_click(token,table,keys,keys_all){

  $('.tabb tr').on('click',function(){
     
    id_c=$(this).attr('id')
    
    requst_inf($(this).attr('id'),table,token,keys,keys_all)
    inf_div=$(this).attr('id')
    $('.inf').addClass('inf_click')

    //del(token,table,$(this).val());
  })
};

function requst_inf(id_el,name,token,keys,keys_all,id_c){
  console.log('id_el=='+id_el+"  id_name=="+name)

  let request_z= 'http://195.46.123.219:22555/?request={"method":"select","params":{"token":"'+token+'","table_name":"'+name+'"}} '
  console.log(request_z)
  $.ajax({
    async:false,
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(){console.log('ошибка')}),
    success:function(data){
    for(i=0;i<data.result.length;i++){
      if(data.result[i].id==id_el){console.log(data.result[i])
      el=data.result[i]}
    }
    inf(el,keys,name,keys_all,id_c);
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

function inf(el,keys,table,keys_all,id_c){
  kol=false
  $('.inf').empty()
  $( '.inf' ).append( '<div class="row"></div>' )
  $( '.inf .row' ).append( '<div class="inf_z col-xl-6"><div class="row inf_str"></div></div> <div class="inf_x col-xl-6" style="overflow:auto; width: 500px; height:1000px;"><div class="row"></div></div>' )

  console.log(el)

  console.log('fffffff='+keys)
  console.log()
  z=keys.length
  for(q=0;q<z;q++){
  
    if(keys[q]=="name"||keys[q]=="univer"){
      $( '.inf_z .inf_str' ).append( '<div class="col-6"><h3>'+el[keys[q]]+'</h3></div>' )
    }else if(keys[q]=="hours"){ 
      $( '.inf_z .inf_str' ).append( '<div class="col-6"><h3>'+el[keys[q]]+' часов</h3></div>' )
    }else if(keys[q]=="id_teacher"){
      teacher(el[keys[q]])
    }else if(keys[q]=="info"){
      info_course(el[keys[q]])
    }else if(keys[q]=="id"&&table=="course"){
      course_groups(el[keys[q]])
      course_plan (el[keys[q]],keys_all)
    }
  }
}

function info_course (info){
  $( '.inf_z' ).append( '<div class="row "><div class="col-4"><h3>описание</h3></div></div>' )
  $( '.inf_z' ).append( '<div class="row info"><div class="col-11"><p>'+info+'</p></div></div>' )


}
function teacher(id){
  let request=1
  let request_z='http://195.46.123.219:22555/?request={"method":"select","params":{"token":"'+token+'","table_name":"user"}} '
  console.log('_____')
  console.log(request_z)
  $.ajax({      
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(data){
      alert('ошибка')
      console.log(data);
    }),
    success:function(data){
      console.log(data)
      for(i=0;i<data.result.length;i++){
        if(data.result[i].id==id){console.log(data.result[i].id)
          $( '.inf_z .inf_str' ).append( '<div class="col-6""><h3>'+data.result[i].name+'  '+data.result[i].surname+'  '+data.result[i].patronymic+'</h3></div>' )}
        }
      }
    
  });
}
function group(id){
  
  let request=1
  let request_z='http://195.46.123.219:22555/?request={"method":"select","params":{"token":"'+token+'","table_name":"group"}} '
  console.log('_____')
  console.log(request_z)
  $.ajax({  
    async:false,  
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(data){
      alert('ошибка')
      console.log(data);
    }),
    success:function(data){
      console.log(data)
        for(z=0;z<data.result.length;z++){
          if(data.result[z].id==id){
            console.log(data.result[z])
             group_name=data.result[z]
            
            
          }
        }
        console.log(group_name)
       return group_name}
   
  });
  
   }
function course_groups(id){
  
  $(".gr").remove()
  let request=1
  let request_z='http://195.46.123.219:22555/?request={"method":"select","params":{"token":"'+token+'","table_name":"course_groups"}} '
  console.log('_____')
  console.log(request_z)
  var g_id=[]
  $.ajax({     
    
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(data){
      alert('ошибка')
      console.log(data);
    }),
    success:function(data){    console.log(data)
      console.log("data")

      
      $( '.inf_z ' ).append( '<div class="col-9"><table class="gr table table-striped table-hover"><thead class="tabt"> </thead><tbody class="tabb"> </tbody></table></div> ' )

      $( '.gr .tabt' ).append( '<tr><th><h1>группы</h1></th><th></th> </tr>' )
        for(e=0;e<data.result.length;e++){
          console.log(e)
          console.log(id)
          console.log("id="+data.result[e].id_course)
          if(data.result[e].id_course==id){
            
            group(data.result[e].id_group)
         console.log(group_name)
           $( '.gr .tabb' ).append( '<tr><th><h3>'+group_name.name+'</h3></th> <th> <button class="del" value='+data.result[e].id+'>удалить</button></th></tr>'  )
           
           g_id.push(group_name.id)
           }
        }
        $('.inf_z .del').on('click',function(){del_c(token,$(this).val(),id);})
        
        $( '.gr' ).append( '<div class="row">'+'<div class="col-10"> <button class="button add_group" data-toggle="modal" data-target="#exampleModal"> добавить группу</button></div></div>'  )
           
$('.add_group').on('click',function(){ 
  form_add_group(g_id)
})

      }
    
  });
}

function del_c(token,zn,id_c){
  console.log(zn)
    let request=1

  let request_z='http://195.46.123.219:22555/?request={"method":"delete","params":{"token":"'+token+'","table_name":"course_groups","rows":['+zn+']}} '

  console.log('_____')
  console.log(request_z)
  $.ajax({      
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(data){
      alert('ошибка')
      console.log(data);
    }),
    success:function(data){
      course_groups(id_c)
      
    }
  });
  }
function form_add_group(g_id){
  let request_z='http://195.46.123.219:22555/?request={"method":"select","params":{"token":"'+token+'","table_name":"group"}} '
  console.log('_____')
  console.log(request_z)
  id_ch=[]
  $.ajax({  
    async:false,  
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(data){
      alert('ошибка')
      console.log(data);
    }),
    success:function(data){
      console.log(data)
      $( ".modal-title" ).empty() 
      $( ".modal-body" ).empty() 
      $( ".modal-footer" ).empty() 
      $( ".modal-title" ).append( 'Добавление ' ) 
      
     

        for(i=0;i<data.result.length;i++){
          if($.inArray(data.result[i].id, g_id)==-1){$( ".modal-body" ).append( '<h3><input id="'+data.result[i].id+'" type="checkbox" name="'+data.result[i].name+'">  '+data.result[i].name+'</h3><br>' )  }
          id_ch.push(data.result[i].id)
        }
        $( ".modal-footer" ).append( ' <div class="but">   <tr> <th><button class="buttonr" data-dismiss="modal">  отмена  </button></th> <th></th><button class="button b_add_gr" data-dismiss="modal">добавить</button></th></tr></div>       ' ) 
      
$('.b_add_gr').on('click',function(){ 
 
 add_g(token)
})
      }
    
  });
}
function add_g(token){
  for(d=0;id_ch.length>d;d++){
    if($('#'+id_ch[d]).is(':checked')==true){
  
      let request_z='http://195.46.123.219:22555/?request={"method":"insert","params":{"token":"'+token+'","table_name":"course_groups","rows":[{'+'"id_course":'+id_c+',"id_group":'+id_ch[d]+',"id":1}]}} '
   console.log('_____')
   console.log(request_z)
   var g_id=[]
   $.ajax({     
     async:false, 
     type:'post',
     url:request_z,
     data: request,
     cach: false,
     error:(function(data){
       alert('ошибка')
       console.log(data);
     }),
     success:function(data){ 

     
       }
     
   });}
 }
  $('.gr').empty() 
      course_groups(id_c)  
}






function course_plan (id,keys_all){
 if(kol==false){
   kol=true;
  $(".pl").remove() 

  let request=1
  let request_z='http://195.46.123.219:22555/?request={"method":"select","params":{"token":"'+token+'","table_name":"course_plan"}} '
  console.log('_____')
  console.log(request_z)
  var s_id=[]
  $.ajax({  
    async:true,    
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(data){
      alert('ошибка')
      console.log(data);
    }),
    success:function(data){   
    
      console.log("iddd==="+id)
      console.log(data)
      console.log("data план")
      $( '.inf_x .row' ).append( '<div class="pl col-12""></div>' )
      $( '.pl' ).append( '<div class="col-2"><h1>план курса</h1></div>' )
      
     for(f=0;f<data.result.length;f++){
       if(data.result[f].id_course==id){
         s_id.push(data.result[f].id_section)
      plan(data.result[f].id_section,keys_all,id_c)}}
      $( ".pl" ).append('<div class="add_sec row"> <a class=" add add_s" data-toggle="modal" data-target="#exampleModal" href="#" >ДОБАВИТЬ раздел</a></div>')
      del_click_t(keys_all,id_c,keys_all)
      $('.add_sec').on('click',function(){ 
        console.log("s_id="+s_id)
        form_add_sec(s_id)
      })
      $('.sec_delt').on('click',function(){ 
       del_sec($(this).val())
      })
    }
    
  }); }
}
function plan(id,keys_all,id_c){
  let request=1
  let request_z='http://195.46.123.219:22555/?request={"method":"select","params":{"token":"'+token+'","table_name":"section"}} '
  console.log('_____')
  console.log(request_z)
  let s_id=[]
  $.ajax({  
    async:false,  
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(data){
      alert('ошибка')
      console.log(data);
    }),
    success:function(data){
      console.log("plan")
      console.log(data)
      console.log(id)
      for(x=0;x<data.result.length;x++){
      if(data.result[x].id==id){
        s_id.push(data.result[x].id)
        cr_sec(data.result[x].id,data.result[x].name,keys_all,id_c)
        //$( '.pl' ).append( '<div class="col-12"><table class="plt table table-striped table-hover"><thead class="tabt"> </thead><tbody class="tabb"> </tbody></table></div> ' )
       // $( '.pl .tabt' ).append( '<tr class="sec '+data.result[x].id+'"><th><h1>'+data.result[x].name+'</h1></th><th>раздел</th> </tr>' )
      //$( '.pl' ).append( '<div class="sec '+data.result[x].id+' row"><div class="sec col-6""><h3>'+data.result[x].name+'</h3></div><div class="col-4"">раздел</div></div> <div class="t row"></div>'  )
     // theme(data.result[x].id,data.result[x].id)
     
      
      }}
    }
   
  });
}
function form_add_sec(g_id){
  let request_z='http://195.46.123.219:22555/?request={"method":"select","params":{"token":"'+token+'","table_name":"section"}} '
  console.log('_____')
  console.log(request_z)
  id_ch=[]
  $.ajax({  
    async:false,  
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(data){
      alert('ошибка')
      console.log(data);
    }),
    success:function(data){
      console.log(data)
      $( ".modal-title" ).empty() 
      $( ".modal-body" ).empty() 
      $( ".modal-footer" ).empty() 
      $( ".modal-title" ).append( 'Добавление ' ) 
      
     

        for(i=0;i<data.result.length;i++){
          if($.inArray(data.result[i].id, g_id)==-1){$( ".modal-body" ).append( '<h3><input id="'+data.result[i].id+'" type="checkbox" name="'+data.result[i].name+'">  '+data.result[i].name+'</h3><br>' )  }
          id_ch.push(data.result[i].id)
        }
        $( ".modal-footer" ).append( ' <div class="but">   <tr> <th><button class="buttonr" data-dismiss="modal">  отмена  </button></th> <th></th><button class="button b_add_gr" data-dismiss="modal">добавить</button></th></tr></div>       ' ) 
      
$('.b_add_gr').on('click',function(){ 
 
 add_s(token)
})
      }
    
  });
}
function add_s(token){
  for(c=0;id_ch.length>c;c++){
    if($('#'+id_ch[c]).is(':checked')==true){
  
      let request_z='http://195.46.123.219:22555/?request={"method":"insert","params":{"token":"'+token+'","table_name":"course_plan","rows":[{'+'"id_course":'+id_c+',"id_section":'+id_ch[c]+',"id":1}]}} '
   console.log('_____')
   console.log(request_z)
   var g_id=[]
   $.ajax({     
     async:false, 
     type:'post',
     url:request_z,
     data: request,
     cach: false,
     error:(function(data){
       alert('ошибка')
       console.log(data);
     }),
     success:function(data){ 
 $('.pl').empty() 
  kol=false
      course_plan (id,keys_all)
       }
     
   });}
 }
 

}

function cr_sec(id_sec,name_sec,keys_all,id_c){

 if(!$("div").is("#plan"+id_sec)){ $( '.pl' ).append( '<div class="col-12  section"  id="plan'+id_sec+'"><div class="row sec"><div class="col-4"><h2>'+name_sec+'</h2></div><div class="col-4"><h4>раздел</h4></div><div class="sec_del col-4"><h4><button class="sec_delt" value='+id_sec+'>удалить</button></h4></div></div><div class="row t"></div></div> ' )//добавить кнопку удалить
  
  $( '#plan'+id_sec+' .t' ).append( '<div class="col-11" background-color="#fff"><table class="plt table table-striped table-hover"><thead class="tabt"><tr><th>тема</th><th>тема</th></tr> </thead><tbody class="tabb"> </tbody></table></div> ' )
  theme(id_sec,keys_all,id_c)}
 
}

function theme(id_sec,keys_all,id_c){

  let request=1
  let request_z='http://195.46.123.219:22555/?request={"method":"select","params":{"token":"'+token+'","table_name":"theme"}} '
  console.log('_____')
  console.log(request_z)
  $.ajax({  
    async:false,  
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(data){
      alert('ошибка')
      console.log(data);
    }),
    success:function(data){

      console.log("plan")
      console.log(data)
      console.log(id_sec)
      for(a=0;a<data.result.length;a++){
      if(data.result[a].id_section==id_sec){
      //$( '.t' ).append( '<div class="col-6"><h3>'+data.result[a].name+'</h3><p>тема</p></div><div class="col-auto"><a class="add" data-toggle="modal" data-target="#exampleModal"  href="#" >удалить тему</a></div></div>'  )
      $( '#plan'+id_sec+' .t .tabb' ).append( '<tr><th><h3>'+data.result[a].name+'</h3></th> <th> <button class="delt" value='+data.result[a].id+'>удалить</button></th></tr>'  )
           
     
     }}
     
     
    $( "#plan"+id_sec ).append('<div class="row"><div class="add_theme" value='+id_sec+'> <a class=" add add_s" data-toggle="modal" data-target="#exampleModal" href="#" >ДОБАВИТЬ тему </a></div></div>')
    $('.add_theme').on('click',function(){ 
           let keys2=Object.keys(data.result[0])
        form(token,"theme",keys2,keys_all);          
        $('.b_add').on('click',function(){add(token,"theme",keys2,keys_all);
        $('.pl').empty() 
        kol=false
        course_plan (id_c,keys_all)})
      
     }) 
  }
   
  });

}
function  del_click_t(keys_all,id_c,keys_all){
  $('tr .delt').on('click',function(){
    
    delt(token,"theme",$(this).val(),keys_all);})
}
function delt(token,table,zn,keys_all){ 
  let request=1
  let request_z='http://195.46.123.219:22555/?request={"method":"delete","params":{"token":"'+token+'","table_name":"'+table+'","rows":['+zn+']}} '
  console.log('_____')
  console.log(request_z)
  $.ajax({      
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(data){
      alert('ошибка')
      console.log(data);
    }),
    success:function(data){
      $('.pl').empty() 
      kol=false
      course_plan (id_c,keys_all)
    }
  });
}


function del_p(token,zn,id_c){
  console.log(zn)
    let request=1
  let request_z='http://195.46.123.219:22555/?request={"method":"delete","params":{"token":"'+token+'","table_name":"course_","rows":['+zn+']}} '
  console.log('_____')
  console.log(request_z)
  $.ajax({      
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(data){
      alert('ошибка')
      console.log(data);
    }),
    success:function(data){

      course_groups(id_c)
      
    }
  });
  }

function add_theme(token){
  for(i=0;id_ch.length>i;i++){
    if($('#'+id_ch[i]).is(':checked')==true){
  
      let request_z='http://195.46.123.219:22555/?request={"method":"insert","params":{"token":"'+token+'","table_name":"course_groups","rows":[{'+'"id_course":'+id_c+',"id_group":'+id_ch[i]+',"id":1}]}} '
   console.log('_____')
   console.log(request_z)
   var g_id=[]
   $.ajax({     
     async:false, 
     type:'post',
     url:request_z,
     data: request,
     cach: false,
     error:(function(data){
       alert('ошибка')
       console.log(data);
     }),
     success:function(data){ 

      $('.pl').empty() 
      course_groups(id_c)
       }
     
   });}
 }
  
}





function form_add_theme(token,g_id){
  let request_z='http://195.46.123.219:22555/?request={"method":"select","params":{"token":"'+token+'","table_name":"theme"}} '
  console.log('_____')
  console.log(request_z)
  id_ch=[]
  $.ajax({  
    async:false,  
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(data){
      alert('ошибка')
      console.log(data);
    }),
    success:function(data){
      console.log(data)
      $( ".modal-title" ).empty() 
      $( ".modal-body" ).empty() 
      $( ".modal-footer" ).empty() 
      $( ".modal-title" ).append( 'Добавление ' ) 
      
     

        for(i=0;i<data.result.length;i++){
          if($.inArray(data.result[i].id, g_id)==-1){$( ".modal-body" ).append( '<h3><input id="'+data.result[i].id+'" type="checkbox" name="'+data.result[i].name+'">  '+data.result[i].name+'</h3><br>' )  }
          id_ch.push(data.result[i].id)
        }
        $( ".modal-footer" ).append( ' <div class="but">   <tr> <th><button class="buttonr" data-dismiss="modal">  отмена  </button></th> <th></th><button class="button b_add_theme" data-dismiss="modal">добавить </button></th></tr></div>       ' ) 
      
$('.b_add_theme').on('click',function(){ 
 
 add_theme(token)
})
}
    
  });
}
function del_click(token,table,keys_all){
    $('tr .del').on('click',function(){del(token,table,$(this).val(),keys_all);})
  };
  
function del(token,table,zn,keys_all){ 
  let request=1
  let request_z='http://195.46.123.219:22555/?request={"method":"delete","params":{"token":"'+token+'","table_name":"'+table+'","rows":['+zn+']}} '
  console.log('_____')
  console.log(request_z)
  $.ajax({      
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(data){
      alert('ошибка')
      console.log(data);
    }),
    success:function(data){
      $( '#tr'+zn ).remove(); 
      page(keys_all,token,table)  
    }
  });
}

function del_sec(id_sec)
{ 
  course_plan_del(id_sec)
  theme_del(id_sec)
  section_del(id_sec)
  re_plan(id_sec)
}


function course_plan_del(id_sec)
{   


  let request_z='http://195.46.123.219:22555/?request={"method":"selekt","params":{"token":"'+token+'","table_name":"course_plan"}} '
  console.log('_____')
  console.log(request_z)
  $.ajax({      
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(data){
      alert('ошибка')
      console.log(data);
    }),
    success:function(data){
    for(s=0;s<data.length;s++){

    }
    }
  });

/*
  let request_z='http://195.46.123.219:22555/?request={"method":"delete","params":{"token":"'+token+'","table_name":"course_plan","rows":["id_section":"'+id_sec+'"]}} '
  console.log('_____')
  console.log(request_z)
  $.ajax({      
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(data){
      alert('ошибка')
      console.log(data);
    }),
    success:function(data){
     alert(11)
    }
  });*/
}


function theme_del(id_sec)
{

}


function section_del(id_sec)
{

}


function re_plan(id_sec)
{

}
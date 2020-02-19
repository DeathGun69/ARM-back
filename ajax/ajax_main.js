
$(document).ready(function(){

  $('.singin').on('click',function(){
    course_table()  //ТАБЛИЦА КУРСОВ
  })
  $('.cours_menu').on('click',function(){
    course_table()  
  })
  $('.add_kurs').on('click',function(){
    add_click() 
  })
  $('.b_add').on('click',function(){add_cours();})
  $('.buttonr').on('click',function(){    })

  $('.group_menu').on('click',function(){
    group_table()
  })
  $('.competenc_menu').on('click',function(){
    competenc_table()   
  })

  $('.b_add_com').on('click',function(){add_com();})
  $('.b_add_group').on('click',function(){add_group();})


})




function add_click(){   
  teacher()
  gr();
  cmp();
  

};
function teacher(){ 
  
  let request=1
  $.ajax({
    type:'post',
    url:'http://195.46.123.219:22555/?request={"method":"select","params":{"table_name":"user"}}',
    data: request,
    cach: false,
    error:(function(){alert('ошибка')}),
    success:function(data_g){    
      console.log(data_g)    
      $( "#tch" ).empty();
      for(i=0;i<data_g.result.length;i++){
        $( "#tch" ).append( ' <option value="'+data_g.result[i].id+'">'+data_g.result[i].name+'</option>' ) 
      }
    }
  });

}
function gr(){ 
  
  let request=1
  $.ajax({
    type:'post',
    url:'http://195.46.123.219:22555/?request={"method":"select","params":{"table_name":"group"}}',
    data: request,
    cach: false,
    error:(function(){alert('ошибка')}),
    success:function(data_g){    
      console.log(data_g)    
      $( "#gr" ).empty();
      for(i=0;i<data_g.result.length;i++){
        $( "#gr" ).append( ' <option value="'+data_g.result[i].id+'">'+data_g.result[i].name+'</option>' ) 
      }
    }
  });

}
function cmp(){
  let request=1
  $.ajax({
    type:'post',
    url:'http://195.46.123.219:22555/?request={"method":"select","params":{"table_name":"competence"}}',
    data: request,
    cach: false,
    error:(function(){alert('ошибка')}),
    success:function(data_g){     
      $( "#s_c" ).empty();
    
      for(i=0;i<data_g.result.length;i++){
        $( "#s_c" ).append( ' <option value="'+data_g.result[i].id+'">'+data_g.result[i].name+'</option>' ) 
      }
    }
  });
}

function add_cours (){
  let request=1
 
  let request_z='http://195.46.123.219:22555/?request={"method":"insert","params":{"table_name":"course","rows":[{"id":'+1+',"name":"'+$('#name').val()+'","group":'+$('#gr').val()+',"id_competence":'+$('#s_c').val()+',"id_teacher":'+$('#tch').val()+',"hours":'+$('#h').val()+',"univer":"'+$('#un').val()+'"}]}} '
  console.log('_____')
  console.log(request_z)
  $.ajax({
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(){alert('ошибка')}),
    success:function(data){ 
    console.log(data)
   alert("курс добавлен")
      course_table()
      ;
 
          
    }
  });
}

function course_table(){  
  clear_table()   //очистка таблици
  add_table()     //заполнение таблицы
  del_click()     // удаление элемента таблицы
}



function del_click(){
  $('.cours_page .delcours').on('click',function(){del_cours($(this).val());})
};

function del_cours (zn){ 
  let request=1
  let request_z='http://195.46.123.219:22555/?request={"method":"delete","params":{"table_name":"course","rows":['+zn+']}} '
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
      $( '.cours_page #tr'+zn ).remove(); 
       }
    });
  }
function clear_table(){
  $( ".cours_page .tabb" ).remove()
}


 
function add_table(){

  $( ".cours_page .table").append( '<tbody class="tabb"></tbody>' )
  let  request={"method":"test"}
  let request_z= 'http://195.46.123.219:22555/?request={"method":"select","params":{"table_name":"course"}} '
  $.ajax({
    async:false,
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(){console.log('ошибка')}),
    success:function(data){
      console.log(data)
      for(i=0;i<data.result.length;i++){
        $( ".cours_page .tabb" ).append( '<tr id="tr'+data.result[i].id+'" class=>')
        $( '.cours_page #tr'+data.result[i].id ).append( '<th>'+data.result[i].name+'</th>' )
        $( ".cours_page #tr"+data.result[i].id ).append( '<th>'+data.result[i].univer+'</th>' )
        $( ".cours_page #tr"+data.result[i].id ).append( '<th>'+data.result[i].hours+'</th>' )


        let request_com= 'http://195.46.123.219:22555/?request={"method":"select","params":{"table_name":"group"}} '
        $.ajax({
          async:false,
          type:'post',
          url:request_com,
          data: request,
          cach: false,
          error:(function(){console.log('ошибка')}),
          success:function(data_c){
            console.log("____")
           console.log( data.result[i].group)
           console.log("_")
          
            console.log(data_c)

            for (z=0;z<data_c.result.length;z++){
             
              console.log(data_c.result[z].id)
              if (data.result[i].group==data_c.result[z].id){
                
            $( ".cours_page #tr"+data.result[i].id ).append( '<th>'+data_c.result[z].name+'</th>');}}
          }
        });
        
        //$( ".cours_page #tr"+data.result[i].id ).append( '<th>'+data.result[i].groups+'</th>' )                     
        request_com= 'http://195.46.123.219:22555/?request={"method":"select","params":{"table_name":"competence","rows":[{"id":'+data.result[i].id_competence+'}]}} '
        $.ajax({
          async:false,
          type:'post',
          url:request_com,
          data: request,
          cach: false,
          error:(function(){console.log('ошибка')}),
          success:function(data_c){
            
            console.log("____")
           console.log( data.result[i].group)
           console.log("_")
          
            console.log(data_c)

            for (z=0;z<data_c.result.length;z++){
             
              console.log(data_c.result[z].id)
              if (data.result[i].id_competence==data_c.result[z].id){
                
            $( ".cours_page #tr"+data.result[i].id ).append( '<th>'+data_c.result[z].name+'</th>');}}
            $( ".cours_page #tr"+data.result[i].id ).append( '<th><button class="buttonr recours" value='+data.result[i].id+'>изменить</button><button class="delcours" value='+data.result[i].id+'>удалить</button></th>' )  
          }
        });
      }    
      $( ".cours_page .tabb" ).append( '</tr>')
    }
  });
}



//группы

function group_table(){  
  clear_table_group()   //очистка таблици
  add_table_group()     //заполнение таблицы
  del_click_group()     // удаление элемента таблицы
}



function del_click_group(){
  $('.group_page .delcours').on('click',function(){del_group($(this).val());})
};

function del_group (zn){ 
  let request=1
  let request_z='http://195.46.123.219:22555/?request={"method":"delete","params":{"table_name":"group","rows":['+zn+']}} '
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
      $( '.group_page #tr'+zn ).remove(); 
       }
    });
  }
function clear_table_group(){
  $( ".group_page .tabb" ).remove()
}


 
function add_table_group(){

  $( ".group_page .table").append( '<tbody class="tabb"></tbody>' )
  let  request={"method":"test"}
  let request_z= 'http://195.46.123.219:22555/?request={"method":"select","params":{"table_name":"group"}} '
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
      for(i=0;i<data.result.length;i++){
        $( ".group_page .tabb" ).append( '<tr id="tr'+data.result[i].id+'" class=>')
        $( '.group_page #tr'+data.result[i].id ).append( '<th>'+data.result[i].name+'</th>' )
        $( ".group_page #tr"+data.result[i].id ).append( '<th>'+data.result[i].code+'</th>' )
        $( ".group_page #tr"+data.result[i].id ).append( '<th><button class="buttonr recours" value='+data.result[i].id+'>изменить</button><button class="delcours" value='+data.result[i].id+'>удалить</button></th>' )
        let request_com= 'http://195.46.123.219:22555/?request={"method":"select","params":{"table_name":"competence","rows":[{"id":'+data.result[i].id_competence+'}]}} '
        
      }   $( ".group_page .tabb" ).append( '</tr>') 
    }
  });
}

function add_group (){
  let request=1
 
  let request_z='http://195.46.123.219:22555/?request={"method":"insert","params":{"table_name":"group","rows":[{"id":'+1+',"name":"'+$('#f3 #name').val()+'","code":'+$('#f3 #un').val()+'}]}} '
  console.log('_____')
  console.log(request_z)
  $.ajax({
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(){alert('ошибка')}),
    success:function(data){ 
    console.log(data)
    alert("группа добавлена")
    group_table()
      
 
          
    }
  });
}





//компетенции

function competenc_table(){  
  clear_table_competenc()   //очистка таблици
  add_table_competenc()     //заполнение таблицы
  del_click_competenc()     // удаление элемента таблицы
}



function del_click_competenc(){

  $('.competenc_page .delcours').on('click',function(){
    del_competenc($(this).val());})
};

function del_competenc (zn){ 
  let request=1
  let request_z='http://195.46.123.219:22555/?request={"method":"delete","params":{"table_name":"competence","rows":['+zn+']}} '
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
      $( '.competenc_page #tr'+zn ).remove(); 
       }
    });
  }
function clear_table_competenc(){
  $( ".competenc_page .tabb" ).remove()
}


 
function add_table_competenc(){

  $( ".competenc_page .table").append( '<tbody class="tabb"></tbody>' )
  let  request={"method":"test"}
  let request_z= 'http://195.46.123.219:22555/?request={"method":"select","params":{"table_name":"competence"}} '
  $.ajax({
    async:false,
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(){console.log('ошибка')}),
    success:function(data){
      console.log(data)
      for(i=0;i<data.result.length;i++){
        $( ".competenc_page .tabb" ).append( '<tr id="tr'+data.result[i].id+'" class=>')
        $( '.competenc_page #tr'+data.result[i].id ).append( '<th>'+data.result[i].name+'</th>' )
        $( ".competenc_page #tr"+data.result[i].id ).append( '<th>'+data.result[i].code+'</th>' )
            $( ".competenc_page #tr"+data.result[i].id ).append( '<th><button class="buttonr recours" value='+data.result[i].id+'>изменить</button><button class="delcours" value='+data.result[i].id+'>удалить</button></th>' )  
                   
       /* let request_com= 'http://195.46.123.219:22555/?request={"method":"select","params":{"table_name":"competence","rows":[{"id":'+data.result[i].id_competence+'}]}} '
        $.ajax({
          async:false,
          type:'post',
          url:request_com,
          data: request,
          cach: false,
          error:(function(){console.log('ошибка')}),
          success:function(data_c){
            console.log(data_c)
            $( ".competenc_page #tr"+data.result[i].id ).append( '<th>'+data_c.result[data.result[i].id_competence-1].name+'</th>');
            $( ".competenc_page #tr"+data.result[i].id ).append( '<th><button class="buttonr recours" value='+data.result[i].id+'>изменить</button><button class="delcours" value='+data.result[i].id+'>удалить</button></th>' )  
          }
        });*/
      }  
        $( ".competenc_page .tabb" ).append( '</tr>')
    }
  });
}
function add_com (){
  let request=1
 
  let request_z='http://195.46.123.219:22555/?request={"method":"insert","params":{"table_name":"competence","rows":[{"id":'+1+',"name":"'+$('#f2 #name').val()+'","code":'+$('#f2 #un').val()+'}]}} '
  console.log('_____')
  console.log(request_z)
  $.ajax({
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(){alert('ошибка')}),
    success:function(data){ 
    console.log(data)
    alert("компетенция добавлена")
    competenc_table()
      
 
          
    }
  });
}

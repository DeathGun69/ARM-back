$('.tabb').ready(function(){
 
   singin();
   $('.add_kurs').on('click',function(){addform();})
});

function singin (){
   let user={
   log : $('#log').val(),
   pas : $('#pas').val()
  } 
  console.log(user);
  console.log(JSON.stringify(user));
 let  request={"method":"test"}
 console.log($);
 let request_z= 'http://195.46.123.219:22555/?request={"method":"select","params":{"table_name":"course"}} '
 console.log(request_z)
 
 
 
 $.ajax({
    async:false,
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(){console.log('ошибка')}),
    success:function(data){
      console.log("zzzzz");   
      console.log(data.result.length);
      console.log(data);
      for(i=0;i<data.result.length;i++){
        $( ".tabb" ).append( '<tr>')
        $( ".tabb" ).append( '<th>'+data.result[i].name+'</th>' )
        $( ".tabb" ).append( '<th>'+data.result[i].univer+'</th>' )
        $( ".tabb" ).append( '<th>'+data.result[i].hours+'</th>' )
        $( ".tabb" ).append( '<th>'+data.result[i].groups+'</th>' )
        let request_com= 'http://195.46.123.219:22555/?request={"method":"select","params":{"table_name":"competence","rows":[{"id":'+data.result[i].id_competence+'}]}} '
       
        $.ajax({
          async:false,
          type:'post',
          url:request_com,
          data: request,
          cach: false,
          error:(function(){console.log('ошибка')}),
          success:function(data_c){
            console.log(data_c)
            
            $( ".tabb" ).append( '<th>'+data_c.result[data.result[i].id_competence-1].name+'</th>' )
            $( ".tabb" ).append( '<th><div class="cl" value="'+data.result[i].id+'"><button class="recours" value>изменить </button><button class="delcours" value="'+data.result[i].id+'">удалить</button></div></th>' )   
            $( ".tabb" ).append( '</tr>')
          }
        });
      }    
      
    }
  });
}
function addform (){

/*$( "head" ).append('<link rel="stylesheet" type="text/css" href="stylef.css" >');
  $( "body" ).append('<div id="back1f">  <div class="containerf"><div class="flex-center1f"><div class="login1f"><h2>Добавление курса</h2><div class="form1f"><p>Название</p><input id="in1f" type="text" placeholder="введите данные">')
  $( "body" ).append('<br><p>Университет</p><input id="in1f" type="text" placeholder="введите данные"><p>Kоличество часов</p><input id="in1f" type="text" placeholder="введите данные">')
  $( "body" ).append('<br><div class="row"><div class="col"><p>группы</p></div><div class="col"></div><img class="imgf" src="https://cdn.icon-icons.com/icons2/1154/PNG/512/1486564412-plus_81511.png" alt="im"></div>')
  $( "body" ).append('</div><select><option>группа 1</option><option>группа 2</option></select><div class="row"><p>компетенции</p><img class="imgf" src="https://cdn.icon-icons.com/icons2/1154/PNG/512/1486564412-plus_81511.png" alt="im">')
  $( "body" ).append('</div><select><option >компетенция 1</option><option >компетенция 2</option></select><div class="row"> <div class="col-4"><button class="buttonrf"> отмена </button></div><div class="col-8"><button class="button"> Авторизация') 
  $( "body" ).append('</button></div></div></div></div></div></div></div>');*/
  
}







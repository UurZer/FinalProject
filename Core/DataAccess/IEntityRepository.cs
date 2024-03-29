﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Core.Entities;

namespace Core.DataAccess
{
    //generic constraint
    //Class: Referans Tip
    //IEntity :T IEntity olabilir veya IEntity implemente eden bir nesne olabilir.
    //new():new'lenebilir olmalı
    public interface IEntityRepository<T> where T:class,IEntity,new()//Sadece Bir class tipi ve IEntity dahil edilen bir class tipi
    {
        List<T> GetAll(Expression<Func<T,bool>> filter=null);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}

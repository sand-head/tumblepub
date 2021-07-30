use activitystreams::{
  collection::{CollectionExt, OrderedCollection, OrderedCollectionPage},
  context,
  object::{ApObject, Page},
  prelude::BaseExt,
  url::Url,
};
use tumblepub_db::models::{blog::Blog, post::Post};
use tumblepub_utils::options::Options;

use crate::ActivityPub;

impl ActivityPub for Post {
  type ApType = ApObject<Page>;

  fn as_activitypub(&self) -> tumblepub_utils::errors::Result<Self::ApType> {
    todo!()
  }
}

impl ActivityPub for (Blog, usize) {
  type ApType = OrderedCollection;

  fn as_activitypub(&self) -> tumblepub_utils::errors::Result<Self::ApType> {
    let (blog, total_posts) = self;
    let local_domain = Options::get().local_domain;
    let mut collection = OrderedCollection::new();

    collection
      .set_context(context())
      .set_id(
        Url::parse(&format!(
          "https://{}/outbox/@{}.json",
          local_domain, blog.name
        ))
        .unwrap(),
      )
      .set_total_items(*total_posts as u64)
      .set_first(
        Url::parse(&format!(
          "https://{}/outbox/@{}.json?page=true",
          local_domain, blog.name
        ))
        .unwrap(),
      );
    Ok(collection)
  }
}

impl ActivityPub for (Blog, Vec<Post>) {
  type ApType = OrderedCollectionPage;

  fn as_activitypub(&self) -> tumblepub_utils::errors::Result<Self::ApType> {
    let (blog, _posts) = self;
    let local_domain = Options::get().local_domain;
    let mut collection = OrderedCollectionPage::new();

    collection.set_context(context()).set_id(
      Url::parse(&format!(
        "https://{}/outbox/@{}.json",
        local_domain, blog.name
      ))
      .unwrap(),
    );
    // .set_many_ordered_items(
    //   self
    //     .iter()
    //     .map(|p| {
    //       Url::parse(&format!(
    //         "https://{}/@{}/posts/{}.json",
    //         local_domain, "blog go here", p.id
    //       ))
    //       .unwrap()
    //     })
    //     .collect::<Vec<_>>(),
    // );
    Ok(collection)
  }
}

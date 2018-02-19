using System.Collections.Generic;

namespace EveOPreview.Mediator.Messages
{
	sealed class ThumbnailListUpdated : NotificationBase<IList<string>>
	{
		public ThumbnailListUpdated(UpdateKind updateKind, IList<string> list)
				: base(list)
		{
			this.Kind = updateKind;
		}

		public UpdateKind Kind { get; }

		public enum UpdateKind
		{
			Add,
			Update,
			Remove
		}
	}
}